
using PdfService.Documents;
using PdfService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Services;

public class PdfProcessorService : IPdfProcessorService
{
    public byte[] PdfContract(Dictionary<string, string> fields)
    {
        var document = new ContractDocument(new ContractModel());

        /*Document document = Document.Create(document =>
        {
            var missingValue = "[missing]";

            document.Page(page =>
            {
                page.Margin(1, Unit.Inch);

                page.Header()
                    .Text("Vertrag zum Bereitstellen einer " + fields.GetValueOrDefault("serviceType", missingValue) 
                    + " zwischen " + fields.GetValueOrDefault("providerLegalName", missingValue) 
                    + " - nachfolgend der Serviceanbieter genannt – " 
                    + " und " + fields.GetValueOrDefault("consumerLegalName", missingValue) + " – nachfolgend der Servicenehmer genannt")
                    .FontSize(48)
                    .FontColor(Colors.Blue.Darken2)
                    .SemiBold();

            page.Header().Text("Bla").FontSize(48)
                    .FontColor(Colors.Blue.Darken2)
                    .SemiBold();

                page.Content().Element(container =>
                {
                    container
                    .PaddingVertical(40)
                    .Height(250)
                    .Background(Colors.Grey.Lighten3)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(Placeholders.LoremIpsum()).FontSize(16);
                });

                /*page.Content()
                    .Background(Colors.Grey.Medium)
                    .Column(column =>
                    {
                        column.Item().Text("Vertrags-ID " + fields.GetValueOrDefault("contractId", missingValue)).FontSize(18);

                        column.Item().Text("§ 1 Vertragsgegenstand").FontSize(18).Bold();
                        column.Item().Text("Der Serviceanbieter verpflichtet sich, folgende Dienstleistungen für den Servicenehmer zu erbringen:").FontSize(18);
                        column.Item().Text("Service ID " + fields.GetValueOrDefault("serviceId", missingValue) + ":").FontSize(18);
                        column.Item().Text("\"" + fields.GetValueOrDefault("serviceName", missingValue) + "\"").FontSize(18);
                        column.Item().Text("Der Vertrag tritt mit dem folgenden Datum in Kraft: " + fields.GetValueOrDefault("creationDate", missingValue)).FontSize(18);

                        column.Item().Text("§ 2 Umfang der Leistungen").FontSize(18).Bold();
                        column.Item().Text("Die übertragenen Dienstleistungen bestehen im Speziellen hieraus:").FontSize(18);
                        column.Item().Text("\"" + fields.GetValueOrDefault("serviceDescription", missingValue) + "\"").FontSize(18);
                        column.Item().Text("Der Serviceanbieter erklärt sich damit einverstanden, die aufgeführten Leistungen fachgerecht vorzunehmen. Die vereinbarte Vergütung bezieht sich ausschließlich auf die an dieser Stelle genannten Dienstleistungen.").FontSize(18);
                        column.Item().Text("Die Daten werden folgendermaßen zur Verfügung gestellt:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("dataAccessType", missingValue)).FontSize(18);

                        column.Item().Text("§ 3 Vergütung").FontSize(18).Bold();
                        column.Item().Text("Die Vergütung der Erbringung des Vertragsgegenstandes ist im Anhang zu finden.").FontSize(18);

                        column.Item().Text("§ 4 Anforderungen an die Hardware").FontSize(18).Bold();
                        column.Item().Text("Folgende Anforderungen an die Hardware müssen erfüllt sein, um den bereitgestellten Dienst in Anspruch zu nehmen:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("hardwareRequirements", missingValue)).FontSize(18);

                        column.Item().Text("§ 5 Laufzeit").FontSize(18).Bold();
                        column.Item().Text("Der Serviceanbieter verpflichtet sich zur Bereitstellung des Dienstes im folgenden Zeitraum:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("contractRuntime", missingValue)).FontSize(18);

                        column.Item().Text("§ 6 Anzahl möglicher Datenaustausche").FontSize(18).Bold();
                        column.Item().Text("Der Serviceanbieter verpflichtet sich, während der Laufzeit des Vertrages bis zu " 
                            + fields.GetValueOrDefault("dataExchangeCount", missingValue) 
                            + " Datenaustausche zu ermöglichen.").FontSize(18);

                        column.Item().Text("§ 7 Vertragsänderungen").FontSize(18).Bold();
                        column.Item().Text("Jedwede Modifizierung dieses Vertrags ist nicht rechtswirksam.").FontSize(18);

                        column.Item().Text("§ 8 Vertragsausfertigung").FontSize(18).Bold();
                        column.Item().Text("Das vorliegende Dokument liegt in digitaler Form vor und kann sowohl vom Serviceanbieter als auch vom Servicenehmer heruntergeladen werden.").FontSize(18);

                        column.Item().Text("§ 9 Erfüllungsort").FontSize(18).Bold();
                        column.Item().Text("Auftragnehmer und Auftraggeber einigen sich darauf, 24161 Altenholz zum Gerichtsstand für die Klärung etwaiger Streitigkeiten aus diesem Vertrag zu machen.").FontSize(18);

                        column.Item().Text("§ 10 Sonstiges").FontSize(18).Bold();
                        column.Item().Text("Der Serviceanbieter bestätigt, dass alle von ihm gemachten Angaben gewissenhaft und wahrheitsgetreu erfolgten. Darüber hinaus verpflichtet er sich, den Servicenehmer über sämtliche vertragsbezogenen Änderungen zeitnah zu informieren.").FontSize(18);
                        column.Item().Text("Folgende weitere Teile sind Bestandteil des Vertrages:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("merlotTncLink", missingValue) + " (Hash: " + fields.GetValueOrDefault("merlotTncHash", missingValue) + ")").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("providerTncLink", missingValue) + " (Hash: " + fields.GetValueOrDefault("providerTncHash", missingValue) + ")").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("attachmentFiles", missingValue)).FontSize(18);

                        column.Item().Text("§ 11 Salvatorische Klausel").FontSize(18).Bold();
                        column.Item().Text("Sollten einzelne Bestimmungen dieses Vertrags ganz oder teilweise unwirksam sein oder werden,bleibt die Wirksamkeit der übrigen Bestimmungen unberührt.").FontSize(18);

                        column.Item().Text("").FontSize(18);

                        column.Item().Text("Der Nutzer " + fields.GetValueOrDefault("providerSignerUser", missingValue) + " hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter " + fields.GetValueOrDefault("providerLegalName", missingValue) + " unterzeichnet:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("providerSignatureDate", missingValue)).FontSize(18);

                        column.Item().Text("Der Nutzer " + fields.GetValueOrDefault("consumerSignerUser", missingValue) + " hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter " + fields.GetValueOrDefault("consumerLegalName", missingValue) + " unterzeichnet:").FontSize(18);
                        column.Item().Text(fields.GetValueOrDefault("consumerSignatureDate", missingValue)).FontSize(18);
                    });
            });
    });*/

        return document.GeneratePdf();
    }
}
