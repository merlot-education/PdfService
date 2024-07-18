/*
 *  Copyright 2024 Dataport. All rights reserved. Developed as part of the MERLOT project.
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

﻿using Microsoft.AspNetCore.Mvc;
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
