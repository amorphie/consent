using System.Text.Json;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.data;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class OpenBankingYosInfoModule : BaseBBTRoute<OBHhsInfoDto, OBHhsInfo, ConsentDbContext>
{
    public OpenBankingYosInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "OpenBankingHhsInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/code/{hhsKod}", GetHhsWithHhsCode);
        routeGroupBuilder.MapPost("PostHhsInfo", PostHhsInfo);
        routeGroupBuilder.MapPost("UpsertHhs/{hhsKod}", UpsertHhs);

    }
    public async Task<IResult> GetHhsWithHhsCode(
   [FromServices] ConsentDbContext context,
   IMapper mapper,
   string hhsKod)
    {
        var hhsInfo = await context.OBHhsInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(y => y.Kod == hhsKod);

        if (hhsInfo != null)
        {
            var hhsInfoDto = mapper.Map<OBHhsInfoDto>(hhsInfo);
            return Results.Ok(hhsInfoDto);
        }
        return Results.NotFound("HhsInfo not found");
    }

    public async Task<IResult> UpsertHhs(
        IMapper mapper,
        [FromServices] ConsentDbContext context,
        [FromBody] OBHhsInfoDto obHhsInfoDto,
        string hhsKod
    )
    {
        var hhsInfo = await context.OBHhsInfos.FirstOrDefaultAsync(x => x.Kod == hhsKod);

        try
        {
            if (hhsInfo != null)
            {
                obHhsInfoDto.Id = hhsInfo.Id;
                mapper.Map(obHhsInfoDto, hhsInfo);

                hhsInfo.LogoBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.logoBilgileri);
                hhsInfo.ApiBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.apiBilgileri);

                context.OBHhsInfos.Update(hhsInfo);
            }
            else
            {
                var newHhsInfo = mapper.Map<OBHhsInfo>(obHhsInfoDto);

                newHhsInfo.LogoBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.logoBilgileri);
                newHhsInfo.ApiBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.apiBilgileri);

                context.OBHhsInfos.Add(newHhsInfo);
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
    public async Task<IResult> PostHhsInfo(
    IMapper mapper,
    [FromServices] ConsentDbContext context,
    IBKMClientService bkmClientService
)
    {
        var accessToken = String.Empty;
        var data = new[]
        {
        new KeyValuePair<string, string>("client_id", "6de7e5fa2a579cd259f143bb7b5ee1ed"),
        new KeyValuePair<string, string>("client_secret", "174aca557d966fc89478d61c991bd7f8"),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("scope", "hhs_read"),
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
            var hhsResponse = await bkmClientService.GetAllHhs(authorizationValue);

        foreach (var hhsDto in hhsResponse)
            {
                var existingHhsInfo = await context.OBHhsInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Kod == hhsDto.kod);

                if (existingHhsInfo != null)
                {
                    var tempId = existingHhsInfo.Id;
                    mapper.Map(hhsDto, existingHhsInfo);
                    existingHhsInfo.Id = tempId;
                }
                else
                {
                    var newHhsInfo = mapper.Map<OBHhsInfo>(hhsDto);
                    context.OBHhsInfos.Add(newHhsInfo);
                }
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }

        return Results.Problem("AccessToken alınamadı.");
    }
}