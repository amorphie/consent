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

    }

     protected async ValueTask<IResult> SearchMethod(
     [FromServices] ConsentDbContext context,
     [FromServices] IMapper mapper,
     [AsParameters] TokenSearch tokenSearch,
     HttpContext httpContext,
     CancellationToken token
 )
    {
        int skipRecords = (tokenSearch.Page - 1) * tokenSearch.PageSize;

        IList<Token> resultList = await context
            .Set<Token>()
            .AsNoTracking()
            .Where(x => string.IsNullOrEmpty(tokenSearch.Keyword) || x.TokenValue.ToLower().Contains(tokenSearch.Keyword.ToLower()))
            .OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(tokenSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<TokenDto>>(resultList))
            : Results.NoContent();
    }
}
