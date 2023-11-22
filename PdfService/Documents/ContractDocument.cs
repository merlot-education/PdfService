using PdfService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfService.Documents;

public class ContractDocument : IDocument
{
    private ContractModel Model { get; }
    private readonly Dictionary<string, string> TypeMapping = new()
    {
        { "merlot:MerlotServiceOfferingDataDelivery" , "Datenlieferung" },
        { "merlot:MerlotServiceOfferingCooperation" , "Kooperation" },
        { "merlot:MerlotServiceOfferingSaaS" , "Webanwendung" },
    };
    private readonly TextStyle CommonTextStyle = TextStyle.Default.FontSize(12);
    private readonly TextStyle CaptionStyle = TextStyle.Default.FontSize(14).Bold();
    private readonly string DateTimeFormatter = "dd.MM.yyyy, HH:mm:ss (\"GMT\"zzz)";
    private readonly int Spacing = 5;

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
        var paragraphIndex = 1;
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(Spacing);

            paragraphIndex = WriteContractMatter(column, paragraphIndex);
            paragraphIndex = WriteContractScope(column, paragraphIndex);
            paragraphIndex = WriteCosts(column, paragraphIndex);
            paragraphIndex = WriteHardwareRequirements(column, paragraphIndex);
            paragraphIndex = WriteContractRuntime(column, paragraphIndex);
            paragraphIndex = WriteDataTransferCount(column, paragraphIndex);
            paragraphIndex = WriteContractChanges(column, paragraphIndex);
            paragraphIndex = WriteContractCopy(column, paragraphIndex);
            paragraphIndex = WriteFulfillmentPlace(column, paragraphIndex);
            paragraphIndex = WriteMiscellaneous(column, paragraphIndex);
            paragraphIndex = WriteEscapeClause(column, paragraphIndex);
            WriteSignatures(column);
        });
    }

    private void WriteCommonContractBlock(ColumnDescriptor column, string caption, List<string> contentLines)
    {
        column.Item().ShowEntire().Text(text =>
        {
            text.ParagraphSpacing(Spacing);
            text.Line(caption).Style(CaptionStyle);
            foreach (string line in contentLines)
            {
                text.Line(line).Style(CommonTextStyle);
            }
        });
    }

    private int WriteContractMatter(ColumnDescriptor column, int paragraphIndex)
    {

        var caption = $"§ {paragraphIndex} Vertragsgegenstand";
        List<string> lines =
        [
            "Der Serviceanbieter verpflichtet sich, folgende Dienstleistungen für den Servicenehmer zu erbringen:",
            $"Service ID {Model.ServiceId} :",
            $"\"{Model.ServiceName}\"",
            $"Der Vertrag tritt mit dem folgenden Datum in Kraft: {Model.ContractCreationDate.ToString(DateTimeFormatter)}"
        ];

        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteContractScope(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Umfang der Leistungen";
        List<string> lines =
        [
            "Die übertragenen Dienstleistungen bestehen im Speziellen hieraus:",
            $"\"{Model.ServiceDescription}\"",
            "Der Serviceanbieter erklärt sich damit einverstanden, die aufgeführten Leistungen fachgerecht vorzunehmen. " +
            "Die vereinbarte Vergütung bezieht sich ausschließlich auf die an dieser Stelle genannten Dienstleistungen."
        ];
        if (Model.ServiceDataAccessType != null
                && !Model.ServiceDataAccessType.Equals("")
                && !Model.ServiceDataAccessType.Equals(ContractModel.MISSING))
        {
            lines.Add("Die Daten werden folgendermaßen zur Verfügung gestellt:");
            lines.Add(Model.ServiceDataAccessType);
        }

        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteCosts(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Vergütung";
        List<string> lines = 
        [
            "Die Vergütung der Erbringung des Vertragsgegenstandes ist im Anhang zu finden."
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteHardwareRequirements(ColumnDescriptor column, int paragraphIndex)
    {
        if (Model.ServiceHardwareRequirements != null
                && !Model.ServiceHardwareRequirements.Equals("")
                && !Model.ServiceHardwareRequirements.Equals(ContractModel.MISSING))
        {
            var caption = $"§ {paragraphIndex} Anforderungen an die Hardware";
            List<string> lines = 
            [
                "Folgende Anforderungen an die Hardware müssen erfüllt sein, um den bereitgestellten Dienst in Anspruch zu nehmen:",
                Model.ServiceHardwareRequirements
            ];
            WriteCommonContractBlock(column, caption, lines);
            return paragraphIndex + 1;
        }
        return paragraphIndex;
    }

    private int WriteContractRuntime(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Laufzeit";
        List<string> lines = 
        [
            "Der Serviceanbieter verpflichtet sich zur Bereitstellung des Dienstes im folgenden Zeitraum:",
            Model.ContractRuntime
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteDataTransferCount(ColumnDescriptor column, int paragraphIndex)
    {
        if (Model.ContractDataTransferCount != null
                && !Model.ContractDataTransferCount.Equals("")
                && !Model.ContractDataTransferCount.Equals(ContractModel.MISSING))
        {
            var caption = $"§ {paragraphIndex} Anzahl möglicher Datenaustausche";
            List<string> lines = 
            [
                $"Der Serviceanbieter verpflichtet sich, während der Laufzeit des Vertrages " +
                $"bis zu {Model.ContractDataTransferCount} Datenaustausche zu ermöglichen.",
            ];
            WriteCommonContractBlock(column, caption, lines);
            return paragraphIndex + 1;
        }
        return paragraphIndex;
    }

    private int WriteContractChanges(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Vertragsänderungen";
        List<string> lines = 
        [
            "Jedwede Modifizierung dieses Vertrags ist nicht rechtswirksam."
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteContractCopy(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Vertragsausfertigung";
        List<string> lines = 
        [
            "Das vorliegende Dokument liegt in digitaler Form vor und kann sowohl vom Serviceanbieter " +
            "als auch vom Servicenehmer heruntergeladen werden."
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteFulfillmentPlace(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Erfüllungsort";
        List<string> lines = 
        [
            "Auftragnehmer und Auftraggeber einigen sich darauf, 24161 Altenholz zum Gerichtsstand für die " +
            "Klärung etwaiger Streitigkeiten aus diesem Vertrag zu machen."
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteMiscellaneous(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Sonstiges";
        List<string> lines = 
        [
            "Der Serviceanbieter bestätigt, dass alle von ihm gemachten Angaben gewissenhaft und wahrheitsgetreu " +
            "erfolgten. Darüber hinaus verpflichtet er sich, den Servicenehmer über sämtliche vertragsbezogenen " +
            "Änderungen zeitnah zu informieren."
        ];
        if (Model.ContractTnc.Length != 0 || Model.ContractAttachmentFilenames.Length != 0)
        {
            lines.Add("Folgende weitere Teile sind Bestandteil des Vertrages:");
            foreach (ContractTncModel tnc in Model.ContractTnc)
            {
                lines.Add($"- AGB: {tnc.TncLink} (Hash: {tnc.TncHash})");
            }
            foreach (string attachmentFilename in Model.ContractAttachmentFilenames)
            {
                lines.Add($"- Anhang: {attachmentFilename}");
            }
        }
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private int WriteEscapeClause(ColumnDescriptor column, int paragraphIndex)
    {
        var caption = $"§ {paragraphIndex} Salvatorische Klausel";
        List<string> lines = 
        [
            "Sollten einzelne Bestimmungen dieses Vertrags ganz oder teilweise unwirksam sein oder " +
            "werden,bleibt die Wirksamkeit der übrigen Bestimmungen unberührt."
        ];
        WriteCommonContractBlock(column, caption, lines);
        return paragraphIndex + 1;
    }

    private void WriteSignatures(ColumnDescriptor column)
    {
        column.Item().ExtendVertical().AlignBottom().ShowEntire().Text(text =>
        {
            text.ParagraphSpacing(Spacing);
            text.Line($"Der Nutzer {Model.ProviderSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ProviderLegalName} unterzeichnet:").Style(CommonTextStyle);
            text.Line($"{Model.ProviderSignatureTimestamp.ToString(DateTimeFormatter)} (Signatur {Model.ProviderSignature})").Style(CommonTextStyle);
            text.Line("");
            text.Line($"Der Nutzer {Model.ConsumerSignerUser} hat den Vertrag an folgendem Datum stellvertretend für den Serviceanbieter {Model.ConsumerLegalName} unterzeichnet:").Style(CommonTextStyle);
            text.Line($"{Model.ConsumerSignatureTimestamp.ToString(DateTimeFormatter)} (Signatur {Model.ConsumerSignature})").Style(CommonTextStyle);
        });
    }
}
