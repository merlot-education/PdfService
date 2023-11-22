using Microsoft.AspNetCore.Mvc;
using PdfService.Models;
using PdfService.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace PdfService.Controllers;

[ApiController]
[Route("/PdfProcessor/")]
public class PdfProcessorController : ControllerBase
{
    protected IWebHostEnvironment WebHostEnvironment { get; init; }
    protected IPdfProcessorService PdfProcessorService { get; init; }

    public PdfProcessorController(IWebHostEnvironment webHostEnvironment,
        IPdfProcessorService pdfProcessorService)
    {
        WebHostEnvironment = webHostEnvironment;
        PdfProcessorService = pdfProcessorService;
    }

    [HttpPost("PdfContract")]
    [Produces("application/pdf")]
    [Consumes("application/json")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public IActionResult PostPdfContract([FromBody] ContractModel model)
    {
        try
        {
            byte[] result = PdfProcessorService.PdfContract(model);

            return new FileStreamResult(new MemoryStream(result), "application/pdf");
        }
        catch (Exception ex)
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
