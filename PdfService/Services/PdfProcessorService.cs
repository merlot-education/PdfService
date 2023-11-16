
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
            var missingValue = "missing";

            document.Page(page =>
            {
                page.Margin(1, Unit.Inch);

                page.Header()
                    .Text("Hello PDF")
                    .FontSize(48)
                    .FontColor(Colors.Blue.Darken2)
                    .SemiBold();

                page.Content()
                    .Background(Colors.Grey.Medium)
                    .Column(column =>
                    {
                        column.Item().Text(Placeholders.LoremIpsum()).FontSize(18);
                        column.Item().Text("Service ID: " + fields.GetValueOrDefault("serviceId", missingValue)).FontSize(18);
                        column.Item().Text("Erstelldatum: " + fields.GetValueOrDefault("creationDate", missingValue)).FontSize(18);
                        column.Item().Text("Serviceanbieter Legal Name: " + fields.GetValueOrDefault("providerLegalName", missingValue)).FontSize(18);
                        column.Item().Text("Serviceanbieter Addresse: " + fields.GetValueOrDefault("providerAddress", missingValue)).FontSize(18);
                        column.Item().Text("Servicename: " + fields.GetValueOrDefault("serviceName", missingValue)).FontSize(18);
                        column.Item().Text("Serviceart: " + fields.GetValueOrDefault("serviceType", missingValue)).FontSize(18);
                        column.Item().Text("Detaillierte Beschreibung des Services: " + fields.GetValueOrDefault("serviceDescription", missingValue)).FontSize(18);
                        column.Item().Text("Datenzugriffsart: " + fields.GetValueOrDefault("dataAccessType", missingValue)).FontSize(18);
                        column.Item().Text("Datentransferart: " + fields.GetValueOrDefault("dataTransferType", missingValue)).FontSize(18);
                        column.Item().Text("Anforderungen an die Hardware: " + fields.GetValueOrDefault("hardwareRequirements", missingValue)).FontSize(18);
                        column.Item().Text("Beispielkosten: " + fields.GetValueOrDefault("exampleCosts", missingValue)).FontSize(18);
                        column.Item().Text("Anbieter AGB Link: " + fields.GetValueOrDefault("providerTncLink", missingValue)).FontSize(18);
                        column.Item().Text("Anbieter AGB Hash: " + fields.GetValueOrDefault("providerTncHash", missingValue)).FontSize(18);
                        column.Item().Text("MERLOT AGB Link: " + fields.GetValueOrDefault("merlotTncLink", missingValue)).FontSize(18);
                        column.Item().Text("MERLOT AGB Hash: " + fields.GetValueOrDefault("merlotTncHash", missingValue)).FontSize(18);
                        // TODO all other AGB?
                        column.Item().Text("Unterschrift Servicenehmer: " + fields.GetValueOrDefault("consumerSignature", missingValue)).FontSize(18);
                        column.Item().Text("ZeitstempelUnterschrift Servicenehmer: " + fields.GetValueOrDefault("consumerSignatureDate", missingValue)).FontSize(18);
                        column.Item().Text("Unterschrift Serviceanbieter: " + fields.GetValueOrDefault("providerSignature", missingValue)).FontSize(18);
                        column.Item().Text("ZeitstempelUnterschrift Serviceanbieter: " + fields.GetValueOrDefault("providerSignatureDate", missingValue)).FontSize(18);
                        // TODO name of signing user?
                    });
            });
        });

        return document.GeneratePdf();
    }
}
