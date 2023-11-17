using PdfService.Models;

namespace PdfService.Services;

public interface IPdfProcessorService
{
    byte[] PdfContract(ContractModel model);
}
