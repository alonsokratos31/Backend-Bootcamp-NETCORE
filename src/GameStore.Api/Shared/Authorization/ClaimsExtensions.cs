using System.Security.Claims;

namespace GameStore.Api.Shared.Authorization;

public static class ClaimsExtensions
{
    public static void TransformScopeClaim(
        this ClaimsIdentity? identity,
        string sourceClaimType
    )
    {
        var scopeClaim = identity?.FindFirst(sourceClaimType);

        if (scopeClaim is null)
        {
            return;
        }

        var scopes = scopeClaim.Value.Split(' ');

        identity?.RemoveClaim(scopeClaim);

        identity?.AddClaims(scopes.Select(scopes => new Claim(GameStoreClaimTypes.Scope, scopes)));
    }


    public static void MapUserIdClaim(
        this ClaimsIdentity? identity,
        string sourceClaimType
    )
    {
        var sourceClaim = identity?.FindFirst(sourceClaimType);

        if (sourceClaim != null)
        {
            identity?.AddClaim(new Claim(GameStoreClaimTypes.UserId, sourceClaim.Value));
        }
    }

    public static void LogAllClaims(
        this ClaimsPrincipal? principal,
        ILogger logger
    )
    {
        var claims = principal?.Claims.ToList();

        if (claims is null)
        {
            return;
        }

        foreach (var claim in claims)
        {
            logger.LogTrace("Claim: {ClaimType}, Value: {ClaimValue}", claim.Type, claim.Value);
        }
    }
}
