/*
 *  Copyright 2023-2024 Dataport AöR
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

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