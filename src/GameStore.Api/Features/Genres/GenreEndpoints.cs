using GameStore.Api.Features.Genres.GetGenres;

namespace GameStore.Api.Features.Genres;

public static class GenreEndpoints
{
    public static void MapGenre(
       this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/genres");
        group.MapGetGenres();
    }

}
