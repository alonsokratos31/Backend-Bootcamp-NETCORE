using Azure.Core;
using Azure.Storage.Blobs;

namespace GameStore.Api.Shared.FileUpload;

public static class FileUploadExtensions
{
    public static void AddFileUploader(this WebApplicationBuilder builder, TokenCredential tokenCredential)
    {
        builder.AddAzureBlobClient("Blobs", settiings =>
        {
            if (builder.Environment.IsProduction())
            {
                settiings.Credential = tokenCredential;
            }
        });
        builder.Services.AddSingleton<FileUploader>();
    }
}
