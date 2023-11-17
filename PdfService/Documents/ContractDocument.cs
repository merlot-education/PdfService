using PdfService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Documents;

public class ContractDocument : IDocument
{
    private ContractModel Model { get; }

    public ContractDocument(ContractModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
        .Page(page =>
        {
            page.Margin(50);

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);


            page.Footer().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold();

        container.ShowOnce().Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().AlignCenter().Text($"Vertrags-ID {Model.ContractId}").FontSize(10);
                column.Item().Text("").FontSize(10);

                column.Item().AlignCenter().Text($"Vertrag zum Bereitstellen einer {Model.ServiceType}").Style(titleStyle);

                column.Item().AlignCenter().Text(text =>
                {
                    text.Span("zwischen ").SemiBold();
                    text.Span($"{Model.ProviderLegalName} - nachfolgend der Serviceanbieter genannt -");
                });

                column.Item().AlignCenter().Text(text =>
                {
                    text.Span("und ").SemiBold();
                    text.Span($"{Model.ConsumerLegalName} - nachfolgend der Servicenehmer genannt -");
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        var textStyle = TextStyle.Default.FontSize(12);
        var captionStyle = TextStyle.Default.FontSize(14).Bold();
        var spacing = 5;
        var padding = 5;
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(spacing);

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 1 Vertragsgegenstand").Style(captionStyle);
                text.Line("Der Serviceanbieter verpflichtet sich, folgende Dienstleistungen für den Servicenehmer zu erbringen:").Style(textStyle);
                text.Line($"Service ID {Model.ServiceId}:").Style(textStyle);
                text.Line($"\"{Model.ServiceName}\"").Style(textStyle);
                text.Line($"Der Vertrag tritt mit dem folgenden Datum in Kraft: {Model.ContractCreationDate}").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 2 Umfang der Leistungen").Style(captionStyle);
                text.Line("Die übertragenen Dienstleistungen bestehen im Speziellen hieraus:").Style(textStyle);
                text.Line($"\"{Model.ServiceDescription}\"").Style(textStyle);
                text.Line("Der Serviceanbieter erklärt sich damit einverstanden, die aufgeführten Leistungen fachgerecht vorzunehmen. Die vereinbarte Vergütung bezieht sich ausschließlich auf die an dieser Stelle genannten Dienstleistungen.").Style(textStyle);
                text.Line("Die Daten werden folgendermaßen zur Verfügung gestellt:").Style(textStyle);
                text.Line(Model.ServiceDataAccessType).Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 3 Vergütung").Style(captionStyle);
                text.Line("Die Vergütung der Erbringung des Vertragsgegenstandes ist im Anhang zu finden.").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 4 Anforderungen an die Hardware").Style(captionStyle);
                text.Line("Folgende Anforderungen an die Hardware müssen erfüllt sein, um den bereitgestellten Dienst in Anspruch zu nehmen:").Style(textStyle);
                text.Line(Model.ServiceHardwareRequirements).Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 5 Laufzeit").Style(captionStyle);
                text.Line("Der Serviceanbieter verpflichtet sich zur Bereitstellung des Dienstes im folgenden Zeitraum:").Style(textStyle);
                text.Line(Model.ContractRuntime).Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 6 Anzahl möglicher Datenaustausche").Style(captionStyle);
                text.Line($"Der Serviceanbieter verpflichtet sich, während der Laufzeit des Vertrages bis zu {Model.ContractDataTransferCount} Datenaustausche zu ermöglichen.").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 7 Vertragsänderungen").Style(captionStyle);
                text.Line("Jedwede Modifizierung dieses Vertrags ist nicht rechtswirksam.").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 8 Vertragsausfertigung").Style(captionStyle);
                text.Line("Das vorliegende Dokument liegt in digitaler Form vor und kann sowohl vom Serviceanbieter als auch vom Servicenehmer heruntergeladen werden.").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 9 Erfüllungsort").Style(captionStyle);
                text.Line("Auftragnehmer und Auftraggeber einigen sich darauf, 24161 Altenholz zum Gerichtsstand für die Klärung etwaiger Streitigkeiten aus diesem Vertrag zu machen.").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 10 Sonstiges").Style(captionStyle);
                text.Line("Der Serviceanbieter bestätigt, dass alle von ihm gemachten Angaben gewissenhaft und wahrheitsgetreu erfolgten. Darüber hinaus verpflichtet er sich, den Servicenehmer über sämtliche vertragsbezogenen Änderungen zeitnah zu informieren.").Style(textStyle);
                if (Model.ContractTnc.Length != 0 || Model.ContractAttachmentFilenames.Length != 0)
                {
                    text.Line("Folgende weitere Teile sind Bestandteil des Vertrages:").Style(textStyle);
                    foreach (ContractTncModel tnc in Model.ContractTnc)
                    {
                        text.Line($"- {tnc.TncLink} (Hash: {tnc.TncHash})").Style(textStyle);
                    }
                    foreach (string attachmentFilename in Model.ContractAttachmentFilenames)
                    {
                        text.Line($"- {attachmentFilename}").Style(textStyle);
                    }
                }
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line("§ 11 Salvatorische Klausel").Style(captionStyle);
                text.Line("Sollten einzelne Bestimmungen dieses Vertrags ganz oder teilweise unwirksam sein oder werden,bleibt die Wirksamkeit der übrigen Bestimmungen unberührt.").Style(textStyle);
            });

            column.Item().Text("");

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"Der Nutzer {Model.ProviderSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ProviderLegalName} unterzeichnet:").Style(textStyle);
                text.Line($"{Model.ProviderSignatureTimestamp} (Signatur {Model.ProviderSignature})").Style(textStyle);
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"Der Nutzer {Model.ConsumerSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ConsumerLegalName} unterzeichnet:").Style(textStyle);
                text.Line($"{Model.ConsumerSignatureTimestamp} (Signatur {Model.ConsumerSignature})").Style(textStyle);
            });
        });
    }
}
