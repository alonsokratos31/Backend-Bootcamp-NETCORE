using Azure.Core;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    private const string postgreSqlScope = "https://ossrdbms-aad.database.windows.net/.default";
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await app.MigrateDbAsync();
        await app.SeedDbAsync();
        app.Logger.LogInformation(14, "The database has been initialized.");
    }

    public static WebApplicationBuilder AddGameStoreNpgsql<TContext>(this WebApplicationBuilder builder,
        string connectionString, TokenCredential credential) where TContext : DbContext
    {
        var connString = builder.Configuration.GetConnectionString(connectionString);

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddNpgsql<TContext>(connString);
        }
        else
        {
            builder.Services.AddNpgsql<TContext>(connString, dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.ConfigureDataSource(dataSpurceBuilder =>
                {
                    dataSpurceBuilder.UsePeriodicPasswordProvider(
                        async (_, cancellationToken) =>
                        {
                            var token = await credential.GetTokenAsync(
                                new TokenRequestContext([postgreSqlScope]),
                                cancellationToken
                            );
                            return token.Token;
                        },
                        TimeSpan.FromHours(24),
                        TimeSpan.FromSeconds(10)
                    );
                });
            });
        }
        return builder;
    }

    private static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<GameStoreContext>();

        if (!await dbContext.Genres.AnyAsync())
        {
            dbContext.Genres.AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "FPS" },
                new Genre { Name = "Sport" }
            );

            await dbContext.SaveChangesAsync();
        }
    }
}
