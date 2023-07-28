using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace amorphie.consent.Module;

public class OpenBankingConsentModule : BaseBBTRoute<OpenBankingConsentDTO, Consent, ConsentDbContext>
{
    public OpenBankingConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/saveConsentData", CustomPost);
        routeGroupBuilder.MapGet("/get", GetConsentWithPermissionsAndToken);
    }



    protected async Task<IResult> CustomPost([FromBody] OpenBankingConsentDTO dto, [FromServices] ConsentDbContext context, [FromServices] IMapper mapper)
    {
        try
        {
            var consent = new Consent
            {
                ConsentDefinitionId = dto.ConsentDefinitionId,
                UserId = dto.UserId,
                State = dto.State,
                ConsentType = dto.ConsentType,
                AdditionalData = dto.AdditionalData,
            };
            context.Consents.Add(consent);
            await context.SaveChangesAsync();

            if (dto.ConsentPermission != null)
            {
                var consentPermission = new ConsentPermission
                {
                    ConsentId = consent.Id,
                    Permission = dto.ConsentPermission.Permission,
                    PermissionLastDate = dto.ConsentPermission.PermissionLastDate,
                    TransactionStartDate = dto.ConsentPermission.TransactionStartDate,
                    TransactionEndDate = dto.ConsentPermission.TransactionEndDate,
                };

                context.ConsentPermissions.Add(consentPermission);
            }

            await context.SaveChangesAsync();


            if (dto.Token != null)
            {
                var tokens = dto.Token.Select(token => new Token
                {
                    ConsentId = consent.Id,
                    TokenType = token.TokenType,
                    TokenValue = token.TokenValue,
                    ExpireTime = token.ExpireTime,
                }).ToList();

                context.Tokens.AddRange(tokens);
            }

            await context.SaveChangesAsync();


            return Results.Ok(consent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<IResult> GetConsentWithPermissionsAndToken(Guid consentId,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {
            var consent = await context.Consents
                .Include(c => c.ConsentPermission)
                .Include(c => c.Token)
                .FirstOrDefaultAsync(c => c.Id == consentId);

            if (consent == null)
            {
                return Results.NotFound("Consent not found.");
            }

            var consentDTO = mapper.Map<OpenBankingConsentDTO>(consent);
            return Results.Ok(consentDTO);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
}
