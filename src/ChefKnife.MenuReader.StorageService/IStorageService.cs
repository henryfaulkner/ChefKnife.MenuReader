namespace ChefKnife.MenuReader.StorageService;

public interface IStorageService
{
    Task<Uri?> UploadAsync(string name, byte[] data, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken = default);
}
