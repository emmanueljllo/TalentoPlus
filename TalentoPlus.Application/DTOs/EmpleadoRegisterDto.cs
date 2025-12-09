using System.ComponentModel.DataAnnotations;
using TalentoPlus.Domain.Enums;

namespace TalentoPlus.Application.DTOs
{
    public class EmpleadoRegisterDto
    {
        [Required] public string Nombres { get; set; } = string.Empty;
        [Required] public string Apellidos { get; set; } = string.Empty;
        [Required] public string DocumentoIdentidad { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CorreoEmpresarial { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
        
        // Datos de catálogo (solo IDs)
        public int CargoId { get; set; }
        public int DepartamentoId { get; set; }
        public int NivelEducativoId { get; set; }
        
        // Otros datos que podrían ser necesarios al registrar
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;
    }
}