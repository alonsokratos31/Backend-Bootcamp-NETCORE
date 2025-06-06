using Azure.Identity;
using GameStore.Api.Data;
using GameStore.Api.Features.Baskets;
using GameStore.Api.Features.Baskets.Authorization;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.Authorization;
using GameStore.Api.Shared.Cdn;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Azure;
using Microsoft.Net.Http.Headers;


Environment.SetEnvironmentVariable("ASPNETCORE_STATICWEBASSETS", "false");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{

    ManagedIdentityClientId = builder.Configuration["AZURE_CLIENT_ID"]
});

builder.AddGameStoreNpgsql<GameStoreContext>("GameStoreDb", credential);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.AddFileUploader(credential);

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

builder.Services.AddSingleton<CdnUrlTransformer>();
builder.Services.AddSingleton<AzureEventSourceLogForwarder>();
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
    app.Services.GetRequiredService<AzureEventSourceLogForwarder>()
                .Start();
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
await app.InitializeDbAsync();


await app.RunAsync();