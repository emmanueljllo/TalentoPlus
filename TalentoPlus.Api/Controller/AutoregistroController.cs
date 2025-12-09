// TalentoPlus.Api/Controllers/AutoregistroController.cs

using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Handlers; 
using TalentoPlus.Application.Exceptions;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http; // Para usar StatusCodes

[Route("api/autoregistro")]
[ApiController]
public class AutoregistroController : ControllerBase
{
    private readonly AutoregistroCommandHandler _registroHandler;

    public AutoregistroController(AutoregistroCommandHandler registroHandler)
    {
        _registroHandler = registroHandler;
    }

    /// <summary>
    /// Permite el autoregistro de un nuevo empleado en el sistema a través de un formulario.
    /// </summary>
    // POST: api/autoregistro
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] EmpleadoRegisterDto request)
    {
        // 1. Validación de Modelo (desde el DTO)
        if (!ModelState.IsValid)
        {
            // Retorna los errores de validación (ej: campo requerido faltante)
            return BadRequest(ModelState);
        }

        try
        {
            // 2. Delega la lógica de negocio al Handler
            await _registroHandler.Handle(request);
            
            // 3. Respuesta exitosa (Creado)
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ConflictException ex)
        {
            // 4. Captura errores de negocio específicos (ej: Documento/Correo duplicado)
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception)
        {
            // 5. Captura cualquier otro error no manejado (ej: error de DB)
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error interno del servidor al intentar el registro." });
        }
    }
}