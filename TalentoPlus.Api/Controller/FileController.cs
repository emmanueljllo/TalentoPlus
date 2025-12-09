using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

[Route("api/files")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IExcelProcessingService _excelService;

    public FileController(IExcelProcessingService excelService)
    {
        _excelService = excelService;
    }

    /// <summary>
    /// Sube un archivo Excel y procesa los datos de los empleados.
    /// </summary>
    [HttpPost("upload/employees")]
    [Consumes("multipart/form-data")] // Indica que este endpoint espera datos de formulario
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // Debe tener el atributo [Authorize] si solo el Admin puede subirlo
    public async Task<IActionResult> UploadEmployeeFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "No se ha subido ningún archivo." });
        }
        
        if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
        {
            return BadRequest(new { Message = "El archivo debe ser de formato Excel (.xlsx o .xls)." });
        }

        try
        {
            using var stream = file.OpenReadStream();
            int processedCount = await _excelService.ProcessEmployeeData(stream);

            return Ok(new 
            { 
                Message = $"Procesamiento completado. Se han añadido {processedCount} nuevos empleados.",
                Count = processedCount
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error al procesar el archivo Excel: " + ex.Message });
        }
    }
}