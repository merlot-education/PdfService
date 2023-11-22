using PdfService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Documents;

public class ContractDocument : IDocument
{
    private ContractModel Model { get; }

    private Dictionary<string, string> TypeMapping = new Dictionary<string, string>()
    {
        { "merlot:MerlotServiceOfferingDataDelivery" , "Datenlieferung" },
        { "merlot:MerlotServiceOfferingCooperation" , "Kooperation" },
        { "merlot:MerlotServiceOfferingSaaS" , "Webanwendung" },
    };

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
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold();

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().AlignCenter().Text(text =>
                {
                    text.Span("Vertrags-ID ").FontSize(10).FontColor(Colors.Grey.Darken2);
                    text.Line(Model.ContractId).FontSize(10).FontColor(Colors.Grey.Darken2).Italic();
                });

                column.Item().ShowOnce().AlignCenter().Text($"Vertrag zum Bereitstellen einer {TypeMapping.GetValueOrDefault(Model.ServiceType, Model.ServiceType)}").Style(titleStyle);
                column.Item().ShowOnce().AlignCenter().Text(text =>
                {
                    text.Span("zwischen  ").SemiBold();
                    text.Span($"{Model.ProviderLegalName} - nachfolgend der Serviceanbieter genannt -");
                });
                column.Item().ShowOnce().AlignCenter().Text(text =>
                {
                    text.Span("und  ").SemiBold();
                    text.Span($"{Model.ConsumerLegalName} - nachfolgend der Servicenehmer genannt -");
                });
            });
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(column =>
        {
            column.Item().AlignCenter().Width(40).Image("merlot_logo.png");
            column.Item().AlignCenter().Text(text =>
            {
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        var textStyle = TextStyle.Default.FontSize(12);
        var textSpecialStyle = TextStyle.Default.FontSize(12).Italic();
        var captionStyle = TextStyle.Default.FontSize(14).Bold();
        var dateTimeFormatter = "dd.MM.yyyy, HH:mm:ss (\"GMT\"zzz)";
        var spacing = 5;

        var paragraphIndex = 1;
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(spacing);

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Vertragsgegenstand").Style(captionStyle);
                text.Line("Der Serviceanbieter verpflichtet sich, folgende Dienstleistungen für den Servicenehmer zu erbringen:").Style(textStyle);
                text.Span("Service ID ").Style(textStyle);
                text.Span(Model.ServiceId).Style(textSpecialStyle);
                text.Line(" :").Style(textStyle);
                text.Line($"\"{Model.ServiceName}\"").Style(textStyle);
                text.Line($"Der Vertrag tritt mit dem folgenden Datum in Kraft: {Model.ContractCreationDate.ToString(dateTimeFormatter)}").Style(textStyle);
                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
            text.ParagraphSpacing(spacing);
            text.Line($"§ {paragraphIndex} Umfang der Leistungen").Style(captionStyle);
            text.Line("Die übertragenen Dienstleistungen bestehen im Speziellen hieraus:").Style(textStyle);
            text.Line($"\"{Model.ServiceDescription}\"").Style(textStyle);
            text.Line("Der Serviceanbieter erklärt sich damit einverstanden, die aufgeführten Leistungen fachgerecht vorzunehmen. Die vereinbarte Vergütung bezieht sich ausschließlich auf die an dieser Stelle genannten Dienstleistungen.").Style(textStyle);

            if (Model.ServiceDataAccessType != null
                && !Model.ServiceDataAccessType.Equals("")
                && !Model.ServiceDataAccessType.Equals(ContractModel.MISSING))
            {
                text.Line("Die Daten werden folgendermaßen zur Verfügung gestellt:").Style(textStyle);
                text.Line(Model.ServiceDataAccessType).Style(textStyle);

                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Vergütung").Style(captionStyle);
                text.Line("Die Vergütung der Erbringung des Vertragsgegenstandes ist im Anhang zu finden.").Style(textStyle);
                paragraphIndex++;
            });

            if (Model.ServiceHardwareRequirements != null
                && !Model.ServiceHardwareRequirements.Equals("")
                && !Model.ServiceHardwareRequirements.Equals(ContractModel.MISSING))
            {
                column.Item().ShowEntire().Text(text =>
                {
                    text.ParagraphSpacing(spacing);
                    text.Line($"§ {paragraphIndex} Anforderungen an die Hardware").Style(captionStyle);
                    text.Line("Folgende Anforderungen an die Hardware müssen erfüllt sein, um den bereitgestellten Dienst in Anspruch zu nehmen:").Style(textStyle);
                    text.Line(Model.ServiceHardwareRequirements).Style(textStyle);
                    paragraphIndex++;
                });
            }


            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Laufzeit").Style(captionStyle);
                text.Line("Der Serviceanbieter verpflichtet sich zur Bereitstellung des Dienstes im folgenden Zeitraum:").Style(textStyle);
                text.Line(Model.ContractRuntime).Style(textStyle);
                paragraphIndex++;
            });

            if (Model.ContractDataTransferCount != null
                && !Model.ContractDataTransferCount.Equals("")
                && !Model.ContractDataTransferCount.Equals(ContractModel.MISSING))
            {
                column.Item().ShowEntire().Text(text =>
                {
                    text.ParagraphSpacing(spacing);
                    text.Line($"§ {paragraphIndex} Anzahl möglicher Datenaustausche").Style(captionStyle);
                    text.Line($"Der Serviceanbieter verpflichtet sich, während der Laufzeit des Vertrages bis zu {Model.ContractDataTransferCount} Datenaustausche zu ermöglichen.").Style(textStyle);
                    paragraphIndex++;
                });
            }


            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Vertragsänderungen").Style(captionStyle);
                text.Line("Jedwede Modifizierung dieses Vertrags ist nicht rechtswirksam.").Style(textStyle);
                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Vertragsausfertigung").Style(captionStyle);
                text.Line("Das vorliegende Dokument liegt in digitaler Form vor und kann sowohl vom Serviceanbieter als auch vom Servicenehmer heruntergeladen werden.").Style(textStyle);
                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Erfüllungsort").Style(captionStyle);
                text.Line("Auftragnehmer und Auftraggeber einigen sich darauf, 24161 Altenholz zum Gerichtsstand für die Klärung etwaiger Streitigkeiten aus diesem Vertrag zu machen.").Style(textStyle);
                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Sonstiges").Style(captionStyle);
                text.Line("Der Serviceanbieter bestätigt, dass alle von ihm gemachten Angaben gewissenhaft und wahrheitsgetreu erfolgten. Darüber hinaus verpflichtet er sich, den Servicenehmer über sämtliche vertragsbezogenen Änderungen zeitnah zu informieren.").Style(textStyle);
                if (Model.ContractTnc.Length != 0 || Model.ContractAttachmentFilenames.Length != 0)
                {
                    text.Line("Folgende weitere Teile sind Bestandteil des Vertrages:").Style(textStyle);
                    foreach (ContractTncModel tnc in Model.ContractTnc)
                    {
                        text.Line($"- AGB: {tnc.TncLink} (Hash: {tnc.TncHash})").Style(textStyle);
                    }
                    foreach (string attachmentFilename in Model.ContractAttachmentFilenames)
                    {
                        text.Line($"- Anhang: {attachmentFilename}").Style(textStyle);
                    }
                }
                paragraphIndex++;
            });

            column.Item().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"§ {paragraphIndex} Salvatorische Klausel").Style(captionStyle);
                text.Line("Sollten einzelne Bestimmungen dieses Vertrags ganz oder teilweise unwirksam sein oder werden,bleibt die Wirksamkeit der übrigen Bestimmungen unberührt.").Style(textStyle);
                paragraphIndex++;
            });

            column.Item().ExtendVertical().AlignBottom().ShowEntire().Text(text =>
            {
                text.ParagraphSpacing(spacing);
                text.Line($"Der Nutzer {Model.ProviderSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ProviderLegalName} unterzeichnet:").Style(textStyle);
                text.Line($"{Model.ProviderSignatureTimestamp.ToString(dateTimeFormatter)} (Signatur {Model.ProviderSignature})").Style(textStyle);
                text.Line("");
                text.Line($"Der Nutzer {Model.ConsumerSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ConsumerLegalName} unterzeichnet:").Style(textStyle);
                text.Line($"{Model.ConsumerSignatureTimestamp.ToString(dateTimeFormatter)} (Signatur {Model.ConsumerSignature})").Style(textStyle);
            });
        });
    }
}
