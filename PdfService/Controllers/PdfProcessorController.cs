using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace PdfService.Controllers;

[ApiController]
[Route("/PdfProcessor/")]
public class PdfProcessorController : ControllerBase
{
    protected IWebHostEnvironment WebHostEnvironment { get; init; }

    public PdfProcessorController(IWebHostEnvironment webHostEnvironment) 
    {
        WebHostEnvironment = webHostEnvironment;
    }

    [HttpPost("PdfContract")]
    [Produces("application/pdf")]
    [Consumes("application/json")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public IActionResult PostPdfContract([FromBody] Dictionary<string, string> data)
    {
        try
        {
            Document document = Document.Create(document =>
                    {
                        document.Page(page =>
                        {
                            page.Margin(1, Unit.Inch);

                            page.Header()
                                .Text("Hello PDF")
                                .FontSize(48)
                                .FontColor(Colors.Blue.Darken2)
                                .SemiBold();

                            page.Content()
                                .Background(Colors.Grey.Medium);

                            page.Content().Column(column =>
                            {
                                column.Item().Text(Placeholders.LoremIpsum()).FontSize(18);
                            });
                        });
                    });

            byte[] myDocByteArray = document.GeneratePdf();

            return Ok(myDocByteArray);
        } 
        catch(Exception ex)
        {
            Debugger.Break();
            return InternalServerError(ex);
        }
    }

    protected ActionResult InternalServerError(Exception ex)
    {
        if (WebHostEnvironment.IsDevelopment())
        {
            return StatusCode(500, ex.Message);
        }

        return StatusCode(500);
    }
}
