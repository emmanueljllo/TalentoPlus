using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Handlers; 
using System;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly LoginCommandHandler _loginHandler;

    public AuthController(LoginCommandHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }

    /// <summary>
    /// Autentica a un usuario (Administrador) y retorna un token JWT.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _loginHandler.Handle(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Credenciales inválidas
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception)
        {
            // Error interno (DB o servicio)
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error interno del servidor." });
        }
    }
    
    // Aquí puedes añadir más endpoints como /api/auth/register o /api/auth/forgot-password
}