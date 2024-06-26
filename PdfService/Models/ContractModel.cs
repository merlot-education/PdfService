﻿namespace PdfService.Models;

public class ContractModel
{
    public static readonly string MISSING = "-";
    public string ContractId { get; set; } = MISSING;
    public DateTimeOffset ContractCreationDate { get; set; } = DateTimeOffset.MinValue;
    public string ContractRuntime { get; set; } = MISSING;
    public string? ContractDataTransferCount { get; set; } = MISSING;
    public string[] ContractAttachmentFilenames { get; set; } = [];
    public ContractTncModel[] ContractTnc { get; set; } = [];

    public string ServiceId { get; set; } = MISSING;
    public string ServiceType { get; set; } = MISSING;
    public string ServiceName { get; set; } = MISSING;
    public string? ServiceDescription { get; set; } = MISSING;
    public string? ServiceDataAccessType { get; set; } = MISSING;
    public string? ServiceDataTransferType { get; set; } = MISSING;
    public string? ServiceHardwareRequirements { get; set; } = MISSING;


    public string ProviderLegalName { get; set; } = MISSING;
    public string ProviderSignerUser { get; set; } = MISSING;
    public DateTimeOffset ProviderSignatureTimestamp { get; set; } = DateTimeOffset.MinValue;
    public string ConsumerSignerUser { get; set; } = MISSING;
    public DateTimeOffset ConsumerSignatureTimestamp { get; set; } = DateTimeOffset.MinValue;
    public string ConsumerLegalName { get; set; } = MISSING;
}