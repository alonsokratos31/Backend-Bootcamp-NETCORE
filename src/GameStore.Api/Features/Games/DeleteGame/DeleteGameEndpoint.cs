using GameStore.Api.Data;
using GameStore.Api.Shared.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", async (Guid id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.AdminAccess);
    }
}
