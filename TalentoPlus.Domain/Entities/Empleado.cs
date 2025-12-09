using System.ComponentModel.DataAnnotations;
using TalentoPlus.Domain.Enums;
using TalentoPlus.Domain.Catologs;

namespace TalentoPlus.Domain.Entities
{
    public class Empleado : BaseEntity
    {
        // Datos Personales
        [Required]
        [MaxLength(100)]
        public required string Nombres { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Apellidos { get; set; }
        
        public DateTime FechaNacimiento { get; set; }
        
        [Required]
        [MaxLength(200)]
        public required string Direccion { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Ciudad { get; set; }
        
        [Required]
        [MaxLength(20)]
        public required string Telefono { get; set; }
        
        // Datos Laborales y de Autenticación
        [Required]
        [MaxLength(50)]
        public required string DocumentoIdentidad { get; set; }
        
        [MaxLength(100)]
        public required string CorreoPersonal { get; set; } // Puede ser nulo o vacío si no se usa

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string CorreoEmpresarial { get; set; } // Usado para login
        
        [Required]
        public required string PasswordHash { get; set; } // Contraseña hasheada (usada por IEmployeePasswordHasher)
        
        public EstadoEmpleado Estado { get; set; } // Usando la enumeración
        
        [Required]
        [MaxLength(500)]
        public required string PerfilProfesional { get; set; }
        
        // Relaciones (Propiedades de Navegación)
        
        // Cargo
        public int CargoId { get; set; }
        public required virtual Cargo Cargo { get; set; } // Required
        
        // Departamento
        public int DepartamentoId { get; set; }
        public required virtual Departamento Departamento { get; set; } // Required
        
        // Nivel Educativo
        public int NivelEducativoId { get; set; }
        public required virtual NivelEducativo NivelEducativo { get; set; } // Required
    }
}