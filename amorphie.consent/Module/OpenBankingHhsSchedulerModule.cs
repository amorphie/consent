using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Enum;
using amorphie.consent.Service.Interface;

namespace amorphie.consent.Module;

public class OpenBankingHhsSchedulerModule : BaseBBTRoute<OpenBankingConsentDto, Consent, ConsentDbContext>
{
    public OpenBankingHhsSchedulerModule(WebApplication app)
        : base(app)
    {
    }

    public override string[] PropertyCheckList => new[] { "ConsentType", "State" };
    public override string UrlFragment => "OpenBankingSchedulerHHS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/SendAccountConsentToAccountService/{consentId}", SendAccountConsentToAccountService);
    }


    protected async Task<bool> SendAccountConsentToAccountService(
        Guid consentId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] ILogger<OpenBankingHhsSchedulerModule> logger)
    {
        bool continueTry = true;
        try
        {
            //Get account consent
            var consentEntity = await context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == consentId
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            var consentDetail = consentEntity?.OBAccountConsentDetails.FirstOrDefault();
            if (consentEntity == null
                || consentDetail == null
                || consentDetail.AccountReferences == null
                || consentDetail.AccountReferences.Count == 0
                || consentDetail.SendToServiceDeliveryStatus != OpenBankingConstants.RecordDeliveryStatus.Processing)
            {
                //No desired consent in system
                continueTry = false;
                return continueTry;
            }

            //Check if event will retry
            int entityTryCount = consentDetail.SendToServiceTryCount ?? 0;
            int maxRetryCount = 10;
            if (entityTryCount < maxRetryCount)
            {
                //Send consent to account service
                var accountRefs = consentDetail.AccountReferences;
                var instanceBalence =
                    consentDetail.PermissionTypes.Contains(OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi)
                        ? OpenBankingConstants.BalanceChangedServiceYesNo.Yes
                        : OpenBankingConstants.BalanceChangedServiceYesNo.No;
                var sharePermission = consentEntity.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi  
                    ? OpenBankingConstants.BalanceChangedServiceYesNo.Yes
                    : OpenBankingConstants.BalanceChangedServiceYesNo.No; 
                var accountServiceResponse = await accountService.SendConsentToAccountService(accountRefs, consentId.ToString(), instanceBalence,sharePermission);
                if (accountServiceResponse.Result) //Success from service
                {
                    //Update consent
                    consentDetail.ModifiedAt = DateTime.UtcNow;
                    consentDetail.SendToServiceLastTryTime = DateTime.UtcNow;
                    consentDetail.SendToServiceTryCount = entityTryCount + 1;
                    consentDetail.SendToServiceDeliveryStatus =
                        OpenBankingConstants.RecordDeliveryStatus.CompletedSuccessfully;
                    continueTry = false;
                }
                else
                {
                    //If error
                    consentDetail.ModifiedAt = DateTime.UtcNow;
                    consentDetail.SendToServiceLastTryTime = DateTime.UtcNow;
                    consentDetail.SendToServiceTryCount = entityTryCount + 1;
                    if (consentDetail.SendToServiceTryCount >= maxRetryCount)
                    {
                        //Mark as undeliverable
                        consentDetail.SendToServiceDeliveryStatus =
                            OpenBankingConstants.RecordDeliveryStatus.Undeliverable;
                        continueTry = false; //sending process completed
                    }
                }
                context.OBAccountConsentDetails.Update(consentDetail);
                await context.SaveChangesAsync();
            }
            else
            {
                //Try count limit exceed. Do not send, set as undeliverable.
                if (consentDetail.SendToServiceDeliveryStatus == OpenBankingConstants.RecordDeliveryStatus.Processing)
                {
                    //Mark as undeliverable
                    consentDetail.SendToServiceDeliveryStatus = OpenBankingConstants.RecordDeliveryStatus.Undeliverable;
                    consentDetail.ModifiedAt = DateTime.UtcNow;
                    context.OBAccountConsentDetails.Update(consentDetail);
                    await context.SaveChangesAsync();
                    continueTry = false; //sending process completed
                }
            }
            return continueTry;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Send account consent to account service.");
            continueTry = true;
            return continueTry;
        }
    }
}