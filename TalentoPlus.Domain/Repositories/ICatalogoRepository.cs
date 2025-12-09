using TalentoPlus.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentoPlus.Domain.Repositories
{
    // Repositorio genérico para catálogos pequeños
    public interface ICatalogoRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}