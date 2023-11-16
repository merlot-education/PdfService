
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Services;

public class PdfProcessorService : IPdfProcessorService
{
    public byte[] PdfContract(Dictionary<string, string> fields)
    {
        Document document = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(1, Unit.Inch);

                page.Header()
                    .Text("Hello PDF")
                    .FontSize(48)
                    .FontColor(Colors.Blue.Darken2)
                    .SemiBold();

                page.Content()
                    .Background(Colors.Grey.Medium);

                page.Content().Column(column =>
                {
                    column.Item().Text(Placeholders.LoremIpsum()).FontSize(18);
                });
            });
        });

        return document.GeneratePdf();
    }
}
