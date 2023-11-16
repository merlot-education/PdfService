using Microsoft.AspNetCore.Mvc.Testing;

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
        Dictionary<string, string> myDict = new();
        myDict.Add("Test1", "Test2");
        byte[]? result = await client.PostJsonAsync<Dictionary<string, string>, byte[]>("/PdfProcessor/PdfContract", myDict);
        Assert.IsNotNull(result);
    }
}
