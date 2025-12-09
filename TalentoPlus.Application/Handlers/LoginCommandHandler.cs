using Microsoft.AspNetCore.Identity;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using System.Threading.Tasks;
using System; 


namespace TalentoPlus.Application.Handlers
{
    public class LoginCommandHandler
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // Utilizamos una excepción estándar de C# para errores de autenticación
                throw new UnauthorizedAccessException("Credenciales inválidas. Usuario no encontrado.");
            }

            // Intenta iniciar sesión con la contraseña (no bloquea la cuenta al fallar: lockoutOnFailure: false)
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas. Contraseña incorrecta.");
            }
            
            // Si el login es exitoso, obtenemos los roles y generamos el token
            var roles = await _userManager.GetRolesAsync(user);
            
            return _jwtService.GenerateToken(user, roles);
        }
    }
}