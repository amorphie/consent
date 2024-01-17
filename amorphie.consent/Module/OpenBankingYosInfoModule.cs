using System.Formats.Asn1;
using System.Net;
using System.Reflection;
using System.Text.Json;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Refit;
namespace amorphie.consent.Module;

public class OpenBankingYosInfoModule : BaseBBTRoute<OBYosInfoDto, OBYosInfo, ConsentDbContext>
{
    public OpenBankingYosInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "OpenBankingYosInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/code/{yosKod}", GetAllYosWithYosCode);
        routeGroupBuilder.MapPost("PostYosInfo", PostYosInfo);
        routeGroupBuilder.MapPost("UpsertYos/{yosKod}", UpsertYos);
    }

    public async Task<IResult> GetAllYosWithYosCode(
     [FromServices] ConsentDbContext context,
     IMapper mapper,
     string yosKod)
    {
        var yosInfo = await context.OBYosInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(y => y.Kod == yosKod);

        if (yosInfo != null)
        {
            var yosInfoDto = mapper.Map<OBYosInfoDto>(yosInfo);
            return Results.Ok(yosInfoDto);
        }
        return Results.NotFound("YosInfo not found");
    }
    public async Task<IResult> UpsertYos(
        IMapper mapper,
        [FromServices] ConsentDbContext context,
        string yosKod,
        IBKMClientService bkmClientService
        )
    {
        var accessToken = String.Empty;
        OBYosInfoDto obYosInfoDto=new OBYosInfoDto();
        var yosInfo = await context.OBYosInfos.FirstOrDefaultAsync(x => x.Kod == yosKod);
        var data = new[]

    {
        new KeyValuePair<string, string>("client_id", "c725c3a61eebefc3c3a1e432ecfdae7d"),
        new KeyValuePair<string, string>("client_secret", "83ade3892b498a3ca7487b0791713539"),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("scope", "yos_read"),
    };
        try
        {
            var httpResponse = await bkmClientService.GetToken(new FormUrlEncodedContent(data));

            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
                accessToken = tokenResponse.AccessToken;

            }
            else
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                return Results.Problem("Token alınamadı.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem("Bir hata oluştu: " + ex.Message);
        }
        if (!string.IsNullOrEmpty(accessToken))
        {
            string authorizationValue = $"Bearer {accessToken}";
            obYosInfoDto = await bkmClientService.GetYos(authorizationValue, yosKod);
        }
        try
        {
            if (yosInfo != null)
            {

                obYosInfoDto.Id=yosInfo.Id;
                mapper.Map(obYosInfoDto, yosInfo);

                yosInfo.Adresler = JsonConvert.SerializeObject(obYosInfoDto.adresler);
                yosInfo.LogoBilgileri = JsonConvert.SerializeObject(obYosInfoDto.logoBilgileri);
                yosInfo.ApiBilgileri = JsonConvert.SerializeObject(obYosInfoDto.apiBilgileri);

                context.OBYosInfos.Update(yosInfo);
            }
            else
            {
                var newYosInfo = mapper.Map<OBYosInfo>(obYosInfoDto);

                newYosInfo.Adresler = JsonConvert.SerializeObject(obYosInfoDto.adresler);
                newYosInfo.LogoBilgileri = JsonConvert.SerializeObject(obYosInfoDto.logoBilgileri);
                newYosInfo.ApiBilgileri = JsonConvert.SerializeObject(obYosInfoDto.apiBilgileri);

                context.OBYosInfos.Add(newYosInfo);
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return Results.NotFound();
        }
    }

    public async Task<IResult> PostYosInfo(
    IMapper mapper,
    [FromServices] ConsentDbContext context,
    [FromServices] IBKMClientService bkmClientService,
    IPushService pushService
)
    {
        var accessToken = String.Empty;
        var data = new[]
        {
        new KeyValuePair<string, string>("client_id", "c725c3a61eebefc3c3a1e432ecfdae7d"),
        new KeyValuePair<string, string>("client_secret", "83ade3892b498a3ca7487b0791713539"),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("scope", "yos_read"),
    };

        try
        {
            var httpResponse = await bkmClientService.GetToken(new FormUrlEncodedContent(data));
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
                accessToken = tokenResponse.AccessToken;
            }
            else
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                return Results.Problem("Token alınamadı.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem("Bir hata oluştu: " + ex.Message);
        }

        if (!string.IsNullOrEmpty(accessToken))
        {
            string authorizationValue = $"Bearer {accessToken}";
            var yosResponse = await bkmClientService.GetAllYos(authorizationValue);

            foreach (var yosDto in yosResponse)
            {
                var existingYosInfo = await context.OBYosInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Kod == yosDto.kod);

                if (existingYosInfo != null)
                {
                    var tempId = existingYosInfo.Id;
                    mapper.Map(yosDto, existingYosInfo);
                    existingYosInfo.Id = tempId;
                }
                else
                {
                    var newYosInfo = mapper.Map<OBYosInfo>(yosDto);
                    context.OBYosInfos.Add(newYosInfo);
                }
            }
            KimlikDto kimlikDto= new KimlikDto{
                kmlkTur="A",
                kmlkVrs="29512549210",
                krmKmlkTur="",
                krmKmlkVrs="",
                ohkTur=""
            };
            await pushService.OpenBankingSendPush(kimlikDto,"23423423423");
            await context.SaveChangesAsync();
            return Results.Ok();
        }

        return Results.Problem("AccessToken alınamadı.");
    }

}