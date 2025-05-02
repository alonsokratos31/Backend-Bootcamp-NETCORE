using Azure.Core;
using Azure.Storage.Blobs;

namespace GameStore.Api.Shared.FileUpload;

public static class FileUploadExtensions
{
    public static void AddFileUploader(this WebApplicationBuilder builder, TokenCredential tokenCredential)
    {
        builder.Services.AddSingleton(serviceProvider =>
                        {
                            var config = serviceProvider.GetRequiredService<IConfiguration>();
                            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();

                            var connectionString = config.GetConnectionString("Blobs")
                                ?? throw new InvalidOperationException("Blob connection string not found.");

                            return environment.IsDevelopment() ?
                                new BlobServiceClient(connectionString) :
                                new BlobServiceClient(
                                    new Uri(connectionString),
                                    tokenCredential);
                        })
                        .AddSingleton<FileUploader>();
    }
}
