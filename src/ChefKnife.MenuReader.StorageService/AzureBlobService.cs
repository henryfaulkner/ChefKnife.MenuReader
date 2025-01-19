using ChefKnife.MenuReader.Shared.Config;
using Azure.Storage.Blobs;
using Azure;

namespace ChefKnife.MenuReader.StorageService;

public class AzureBlobService : IStorageService
{
    readonly AzureBlobConfig _config;
    readonly BlobServiceClient _blobServiceClient;
    readonly BlobContainerClient _containerClient;

    public AzureBlobService(AzureBlobConfig config)
    {
        _config = config;
        _blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(config.ContainerName);
    }

    public async Task<Uri?> UploadAsync(string blobName, byte[] data, CancellationToken cancellationToken = default)
    {
        Uri? result = null;

        // Ensure the container exists
        _containerClient.CreateIfNotExists(cancellationToken: cancellationToken); 

        // Get a reference to the blob
        BlobClient blobClient = _containerClient.GetBlobClient(blobName);

        // Upload the PDF data as a blob
        using MemoryStream stream = new MemoryStream(data);

        // Upload will not overwrite existing blob data
        try
        {
            await blobClient.UploadAsync(stream, overwrite: false, cancellationToken: cancellationToken);
            result = blobClient.Uri;
        }
        catch (RequestFailedException ex)
        {
            if (ex.ErrorCode == "BlobAlreadyExists") result = blobClient.Uri;
        }

        return result;
    }

    public async Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(uri, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to download file at {uri.ToString()}. Status code: {response.StatusCode}");
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
