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

    #region TokenModule SearchMethod
    // This method retrieves a list of tokens from the database based on the provided search criteria.
    // It performs filtering and pagination in the database using the search criteria.
    // If there are results, it returns them as a successful result with a list of TokenDto objects.
    // If there are no results, it returns a no content result.
    protected async ValueTask<IResult> SearchMethod(
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper,
    [AsParameters] TokenSearch tokenSearch,
    CancellationToken token
)
{
    int skipRecords = (tokenSearch.Page - 1) * tokenSearch.PageSize;

    IQueryable<Token> query = context.Tokens.AsNoTracking();

    if (!string.IsNullOrEmpty(tokenSearch.Keyword))
    {
        string keyword = tokenSearch.Keyword.ToLower();
          query = query.AsNoTracking().Where(x => EF.Functions.ToTsVector("english", string.Join(" ", x.TokenValue, x.TokenType))
           .Matches(EF.Functions.PlainToTsQuery("english", tokenSearch.Keyword)));
    }

    IList<Token> resultList = await query.OrderBy(x => x.CreatedAt)
        .Skip(skipRecords)
        .Take(tokenSearch.PageSize)
        .ToListAsync(token);

    return (resultList != null && resultList.Count > 0)
        ? Results.Ok(mapper.Map<IList<TokenDto>>(resultList))
        : Results.NoContent();
}
    #endregion
}
