using System;
using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData
{
    private readonly List<Genre> genres = [
        new Genre { Id = new Guid("d35ed94b-17a2-4002-9055-d55d2b799f20"), Name= "Fighting"},
        new Genre { Id = new Guid("211ed3bd-c8f8-4638-a546-66c4266998cd"), Name= "RPG"},
        new Genre { Id = new Guid("9db91dbb-77f5-4149-89e0-088e1a945c27"), Name= "Adventure"},
        new Genre { Id = new Guid("ba55c1f7-3f38-4d3f-8bd8-1c4e139a2cd2"), Name= "FPS"}
    ];


    private readonly List<Game> games;

    public GameStoreData()
    {
        games = [
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Mortal Kombat",
                Genre = genres[0],
                GenreId = genres[0].Id,
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description = "Mejor juego de pelea",
                ImageUri = "https://example.com/mortal_kombat.jpg",
                LastUpdatedBy = "admin"

            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy ",
                Genre = genres[1],
                GenreId = genres[1].Id,
                Price = 39.99m,
                Description = "Mejor juego de RPG",
                ImageUri = "https://example.com/final_fantasy.jpg",
                LastUpdatedBy = "admin"


            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Uncharted",
                Genre = genres[2],
                GenreId = genres[2].Id,
                Description = "Mejor juego de aventura",
                ImageUri = "https://example.com/uncharted.jpg",
                ReleaseDate = new DateOnly(2008, 7, 15),
                LastUpdatedBy = "admin"
            }

        ];
    }

    public IEnumerable<Game> GetGames() => games;

    public Game? GetGame(Guid id) => games.Find(game => game.Id == id);

    public void AddGame(Game game)
    {

        game.Id = Guid.NewGuid();
        games.Add(game);

    }

    public void RemoveGame(Guid id)
    {
        games.RemoveAll(game => game.Id == id);
    }

    public IEnumerable<Genre> GetGenres() => genres;

    public Genre? GetGenre(Guid id) => genres.Find(genre => genre.Id == id);

}
