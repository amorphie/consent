using amorphie.consent.core.DTO;
using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using Dapr;

namespace amorphie.consent.Module;

public class SchedulerModule : BaseBBTRoute<TokenDto, Token, ConsentDbContext>
{
    public SchedulerModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "TokenType", "TokenValue" };

    public override string? UrlFragment => "scheduler";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("amorphie-scheduler", Scheduler);
    }
    [HttpPost("/amorphie-scheduler")]
    public async Task<IResult> Scheduler(
        PushService pushService
   )
    {
        Console.WriteLine("Scheduler çalıştı.");

        return Results.Ok("Scheduler Çalıştı");
    }

}
