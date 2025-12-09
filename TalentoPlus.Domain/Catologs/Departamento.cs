using System.ComponentModel.DataAnnotations;
using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Domain.Catologs
{
    public class Departamento : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public required string Nombre { get; set; }
        
        [MaxLength(300)]
        public string? Descripcion { get; set; } 
        
        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}