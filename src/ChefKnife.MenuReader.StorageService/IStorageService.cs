namespace ChefKnife.MenuReader.StorageService;

public interface IStorageService
{
    Task<Uri> UploadAsync(string name, byte[] data);
    Task<byte[]> DownloadAsync(Uri uri);
}
