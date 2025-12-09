using System.IO;
using System.Threading.Tasks;

namespace TalentoPlus.Application.Interfaces
{
    public interface IExcelProcessingService
    {
        // El Stream contiene el archivo subido desde el frontend/API
        Task<int> ProcessEmployeeData(Stream fileStream);
    }
}