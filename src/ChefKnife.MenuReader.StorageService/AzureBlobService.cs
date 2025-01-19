using ChefKnife.MenuReader.Shared.Config;
using Azure.Storage.Blobs;

namespace ChefKnife.MenuReader.StorageService;

public class AzureBlobService : IStorageService
{
    readonly BlobServiceClient _blobServiceClient;
    readonly BlobContainerClient _containerClient;

    public AzureBlobService(AzureBlobConfig config)
    {
        _blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(config.ContainerName);

        // Ensure the container exists
        _containerClient.CreateIfNotExists();
    }

    public async Task<Uri> UploadAsync(string blobName, byte[] data)
    {
        // Get a reference to the blob
        BlobClient blobClient = _containerClient.GetBlobClient(blobName);

        // Upload the PDF data as a blob
        using MemoryStream stream = new MemoryStream(data);

        // Upload will not overwrite existing blob data
        await blobClient.UploadAsync(stream, overwrite: false);

        return blobClient.Uri;
    }

    public async Task<byte[]> DownloadAsync(Uri uri)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to download file at {uri.ToString()}. Status code: {response.StatusCode}");
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
}
