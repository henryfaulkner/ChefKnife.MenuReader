namespace ChefKnife.MenuReader.DocumentProcessorService;

public interface IDocumentProcessorService
{
    Task<Dictionary<string, ProcessedField>> ExtractDataFromDocumentAsync(Stream stream, CancellationToken cancellationToken = default);
}
