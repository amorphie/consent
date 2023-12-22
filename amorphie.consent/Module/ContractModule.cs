using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Contract;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Module;

public class ContractModule : BaseBBTRoute<ConsentDto, Consent, ConsentDbContext>
{
    public ContractModule(WebApplication app)
        : base(app)
    {
    }
    public override string[]? PropertyCheckList => null;
    public override string? UrlFragment => "Authorization";
    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/instance", ContractInstance);
        
    }
    public async Task<IResult> ContractInstance([FromBody] InstanceRequestDto instanceRequest,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IContractService contractService,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            ApiResult contractApiResult = await contractService.ContractInstance(instanceRequest);
            if (!contractApiResult.Result)
            {
                return Results.BadRequest(contractApiResult.Message);
            }
            return Results.Ok(contractApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
}