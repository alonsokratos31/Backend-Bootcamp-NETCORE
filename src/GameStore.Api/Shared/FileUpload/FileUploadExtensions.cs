using Azure.Storage.Blobs;

namespace GameStore.Api.Shared.FileUpload;

public static class FileUploadExtensions
{
    public static void AddFileUploader(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(serviceProvider =>
                        {
                            var config = serviceProvider.GetRequiredService<IConfiguration>();
                            var connectionString = config.GetConnectionString("Blobs");
                            return new BlobServiceClient(connectionString);
                        })
                        .AddSingleton<FileUploader>();
    }
}
