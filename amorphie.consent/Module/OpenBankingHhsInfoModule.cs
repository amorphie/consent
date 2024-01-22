using System.Text.Json;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using amorphie.core.Base;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class OpenBankingHhsInfoModule : BaseBBTRoute<OBHhsInfoDto, OBHhsInfo, ConsentDbContext>
{
    public OpenBankingHhsInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "OpenBankingHhsInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/code/{hhsKod}", GetHhsWithHhsCode);
        // routeGroupBuilder.MapPost("PostHhsInfo", PostHhsInfo);
        routeGroupBuilder.MapPost("UpsertHhs/{hhsKod}", UpsertHhs);
        routeGroupBuilder.MapPost("PosAllHhsInfo", PosAllHhsInfo);

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
        IBKMService bkmService,
        [FromServices] ConsentDbContext context,
        string hhsKod
    )
    {
        var hhsInfo = await context.OBHhsInfos.FirstOrDefaultAsync(x => x.Kod == hhsKod);
        var data = await bkmService.GetHhs(hhsKod);
        OBHhsInfoDto oBHhsInfoDto = new();
        var obHhsInfoDto = mapper.Map(data, oBHhsInfoDto);

        try
        {
            if (hhsInfo != null)
            {
                obHhsInfoDto.Id = hhsInfo.Id;
                mapper.Map(obHhsInfoDto, hhsInfo);

                hhsInfo.LogoBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.logoBilgileri);
                hhsInfo.ApiBilgileri = JsonConvert.SerializeObject(obHhsInfoDto.apiBilgileri);
                hhsInfo.ModifiedAt=DateTime.Now.ToUniversalTime();
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



    public async Task<IResult> PosAllHhsInfo(
        IMapper mapper,
        IBKMService bkmService,
        [FromServices] ConsentDbContext context
    )
    {
        var data = await bkmService.GetAllHhs();
        List<OBHhsInfoDto> dtos = new();
        var test = mapper.Map(data, dtos);

        try
        {
            foreach (var hhsDto in test)
            {
                var existingHhsInfo = await context.OBHhsInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Kod == hhsDto.kod);

                if (existingHhsInfo != null)
                {
                    var tempId = existingHhsInfo.Id;
                    mapper.Map(hhsDto, existingHhsInfo);
                    existingHhsInfo.Id = tempId;
                    existingHhsInfo.LogoBilgileri = JsonConvert.SerializeObject(hhsDto.logoBilgileri);
                    existingHhsInfo.ApiBilgileri = JsonConvert.SerializeObject(hhsDto.apiBilgileri);
                    context.Update(existingHhsInfo);
                }
                else
                {
                    var newHhsInfo = mapper.Map<OBHhsInfo>(hhsDto);
                    newHhsInfo.LogoBilgileri = JsonConvert.SerializeObject(hhsDto.logoBilgileri);
                    newHhsInfo.ApiBilgileri = JsonConvert.SerializeObject(hhsDto.apiBilgileri);
                    context.OBHhsInfos.Add(newHhsInfo);
                }
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {

            return Results.Problem(ex.Message);
        }

    }
}

//     public async Task<IResult> PostHhsInfo(
//     IMapper mapper,
//     [FromServices] ConsentDbContext context,
//     IBKMClientService bkmClientService,
//     IConfiguration configuration
// )
//     {
//         var clientId = configuration["ClientId:HhsClientId"];
//         var clientSecret = configuration["ClientSecret:HhsClientSecret"];
//         var accessToken = String.Empty;
//         var data = new BKMTokenRequestDto
//         {
//             ClientId = clientId,
//             ClientSecret = clientSecret,
//             GrantType = "client_credentials",
//             Scope = "hhs_read"
//         };
//         try
//         {
//             var httpResponse = await bkmClientService.GetToken(data);
//             if (httpResponse.IsSuccessStatusCode)
//             {
//                 var content = await httpResponse.Content.ReadAsStringAsync();
//                 var tokenResponse = JsonConvert.DeserializeObject<BKMTokenResponseDto>(content);
//                 accessToken = tokenResponse.AccessToken;
//             }
//             else
//             {
//                 var errorContent = await httpResponse.Content.ReadAsStringAsync();
//                 Console.WriteLine(errorContent);
//                 return Results.Problem("Token alınamadı.");
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine(ex.Message);
//             return Results.Problem("Bir hata oluştu: " + ex.Message);
//         }

//         if (!string.IsNullOrEmpty(accessToken))
//         {
//             string authorizationValue = $"Bearer {accessToken}";
//             var hhsResponse = await bkmClientService.GetAllHhs(authorizationValue);

//             foreach (var hhsDto in hhsResponse)
//             {
//                 var existingHhsInfo = await context.OBHhsInfos
//                     .AsNoTracking()
//                     .FirstOrDefaultAsync(x => x.Kod == hhsDto.kod);

//                 if (existingHhsInfo != null)
//                 {
//                     var tempId = existingHhsInfo.Id;
//                     mapper.Map(hhsDto, existingHhsInfo);
//                     existingHhsInfo.Id = tempId;
//                 }
//                 else
//                 {
//                     var newHhsInfo = mapper.Map<OBHhsInfo>(hhsDto);
//                     context.OBHhsInfos.Add(newHhsInfo);
//                 }
//             }

//             await context.SaveChangesAsync();
//             return Results.Ok();
//         }

//         return Results.Problem("AccessToken alınamadı.");
//     }