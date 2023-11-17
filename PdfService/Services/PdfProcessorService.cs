
using PdfService.Documents;
using PdfService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Services;

public class PdfProcessorService : IPdfProcessorService
{
    public byte[] PdfContract(ContractModel model)
    {
        var document = new ContractDocument(model);
        return document.GeneratePdf();
    }
}
