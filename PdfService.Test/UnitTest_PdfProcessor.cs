using Microsoft.AspNetCore.Mvc.Testing;
using PdfService.Models;

namespace PdfService.Test;

[TestClass]
public class UnitTest_PdfProcessor
{
    protected WebApplicationFactory<Program> ApplicationFactory { get; init; }

    public UnitTest_PdfProcessor()
    {
        ApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            //builder.UseEnvironment("Test");
        });
    }

    [TestMethod]
    public async Task GenerateContractPdf()
    {
        HttpClient client = ApplicationFactory.CreateClient();
        var model = new ContractModel()
        {
            ContractId = "Contract:1357",
            ContractCreationDate = new DateTimeOffset(2023, 01, 01, 11, 11, 11, TimeSpan.FromHours(1)),
            ContractRuntime = "5 Tage",
            ContractDataTransferCount = "10",
            ContractAttachmentFilenames = ["MeineTolleDatei.pdf", "MeineAndereTolleDatei.pdf"],
            ContractTnc = [
                new ContractTncModel() { TncLink = "http://example.com", TncHash = "hash1234" },
                new ContractTncModel() { TncLink = "http://merlot-education.eu", TncHash = "hash1357" }
            ],

            ServiceId = "ServiceOffering:1234",
            ServiceName = "Mein Dienst",
            ServiceType = "Datenlieferung",
            ServiceDescription = "Liefert Daten von A nach B",
            ServiceDataAccessType = "Download",
            ServiceHardwareRequirements = "Flux-Kondensator mit 1.21 Gigawatt",

            ProviderLegalName = "MeinAnbieter GmbH",
            ProviderSignerUser = "Hans Wurst",
            ProviderSignature = "12345678",
            ProviderSignatureTimestamp = new DateTimeOffset(2023, 01, 01, 11, 11, 11, TimeSpan.FromHours(1)),

            ConsumerLegalName = "Konsum AG",
            ConsumerSignerUser = "Marco Polo",
            ConsumerSignature = "87654321",
            ConsumerSignatureTimestamp = new DateTimeOffset(2023, 01, 01, 11, 11, 11, TimeSpan.FromHours(1)),
        };
        byte[]? result = await client.PostJsonAsync<ContractModel, byte[]>("/PdfProcessor/PdfContract", model);
        Assert.IsNotNull(result);
    }
}
