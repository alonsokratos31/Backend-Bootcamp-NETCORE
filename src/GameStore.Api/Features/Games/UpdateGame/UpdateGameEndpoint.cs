using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT/games/23265659-54554-544
        app.MapPut("/{id}", async (Guid id, [FromForm] UpdateGameDto updatedGame,
                                  GameStoreContext dbContext, FileUploader fileUploader) =>
        {
            Game? existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            if (updatedGame.ImageFile is not null)
            {
                var fileUploaderResult = await fileUploader.UploadFileAsync(updatedGame.ImageFile, StorageNames.GameImageFolder);

                if (!fileUploaderResult.IsSucess)
                {
                    return Results.BadRequest(new { message = fileUploaderResult.ErrorMessage });
                }

                existingGame.ImageUri = fileUploaderResult.FileUrl!;
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;
            existingGame.Description = updatedGame.Description;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithParameterValidation()
        .DisableAntiforgery();
    }

}
