using Azure.Storage.Blobs;
using GameStore.Api.Data;
using GameStore.Api.Features.Baskets;
using GameStore.Api.Features.Baskets.Authorization;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.Authorization;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddFileUploader();

builder.AddGameStoreAuthentication();
builder.AddGameStoreAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var allowedOrigin = "http://localhost:5173";
            policy.WithOrigins(allowedOrigin)
                  .WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors();
app.UseAuthorization();

app.MapGames();
app.MapGenre();
app.MapBaskets();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
await app.InitializeDbAsync();


await app.RunAsync();