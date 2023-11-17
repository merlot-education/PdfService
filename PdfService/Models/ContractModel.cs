namespace PdfService.Models;

public class ContractModel
{
    private static string MISSING = "[missing]";
    public string ContractId { get; set; } = MISSING;
    public string ContractCreationDate { get; set; } = MISSING;
    public string ContractRuntime { get; set; } = MISSING;
    public string ContractDataTransferCount { get; set; } = MISSING;
    public string[] ContractAttachmentFilenames { get; set; } = [];
    public ContractTncModel[] ContractTnc { get; set; } = [];

    public string ServiceId { get; set; } = MISSING;
    public string ServiceType { get; set; } = MISSING;
    public string ServiceName {  get; set; } = MISSING;
    public string ServiceDescription { get; set; } = MISSING;
    public string ServiceDataAccessType { get; set; } = MISSING;
    public string ServiceHardwareRequirements { get; set; } = MISSING;


    public string ProviderLegalName { get; set; } = MISSING;
    public string ProviderSignerUser { get; set; } = MISSING;
    public string ProviderSignature { get; set; } = MISSING;
    public string ProviderSignatureTimestamp { get; set; } = MISSING;
    public string ConsumerSignerUser { get; set; } = MISSING;
    public string ConsumerSignature { get; set; } = MISSING;
    public string ConsumerSignatureTimestamp { get; set; } = MISSING;
    public string ConsumerLegalName { get; set; } = MISSING;
}