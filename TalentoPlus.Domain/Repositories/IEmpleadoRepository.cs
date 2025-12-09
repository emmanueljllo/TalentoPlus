using TalentoPlus.Domain.Entities;
using System.Collections.Generic;
using TalentoPlus.Domain.Repositories;
using System.Threading.Tasks;

namespace TalentoPlus.Domain.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<Empleado> GetByIdAsync(int id);
        Task AddAsync(Empleado empleado);
        Task<List<Empleado>> GetAllAsync();
    }
}

