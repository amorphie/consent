using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;

namespace amorphie.consent.Module;

public class TokenModule : BaseBBTRoute<TokenDto, Token, ConsentDbContext>
{
    public TokenModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "TokenType", "TokenValue" };

    public override string? UrlFragment => "token";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);

        routeGroupBuilder.MapGet("/search", SearchMethod);

        routeGroupBuilder.MapGet("/custom-method", CustomMethod);
    }


    [AddSwaggerParameter("Test Required", ParameterLocation.Header, true)]
    protected async ValueTask<IResult> CustomMethod()
    {
        return Results.Ok();
    }

    protected async ValueTask<IResult> SearchMethod(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [AsParameters] TokenSearch consentSearch,
        HttpContext httpContext,
        CancellationToken token
    )
    {
        IList<Token> resultList = await context
            .Set<Token>()
            .AsNoTracking()
            .Where(
                x =>
                    x.TokenValue.Contains(consentSearch.Keyword!)
                    || x.TokenValue.Contains(consentSearch.Keyword!)
            )
            .Skip(consentSearch.Page)
            .Take(consentSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<TokenDto>>(resultList))
            : Results.NoContent();
    }
}
