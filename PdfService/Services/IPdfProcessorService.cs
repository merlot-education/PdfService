namespace PdfService.Services;

public interface IPdfProcessorService
{
    byte[] PdfContract(Dictionary<string, string> fields);
}
