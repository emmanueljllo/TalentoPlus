using TalentoPlus.Domain.Repositories;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentoPlus.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Empleado empleado)
        {
            await _context.Empleados.AddAsync(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Empleado empleado)
        {
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailOrDocumentoAsync(string email, string documento)
        {
            return await _context.Empleados
                .AnyAsync(e => e.CorreoEmpresarial == email || e.DocumentoIdentidad == documento);
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public async Task<Empleado?> GetByEmailAsync(string email)
        {
            return await _context.Empleados.FirstOrDefaultAsync(e => e.CorreoEmpresarial == email);
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task UpdateAsync(Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
        }
    }
}