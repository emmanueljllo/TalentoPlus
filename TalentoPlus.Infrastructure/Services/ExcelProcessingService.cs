using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Repositories;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Catologs;
using TalentoPlus.Domain.Enums;
using ExcelDataReader;
using System.IO;
using System.Threading.Tasks;
using System.Text.Encoding;
using System.Linq;

namespace TalentoPlus.Infrastructure.Services
{
    public class ExcelProcessingService : IExcelProcessingService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        
        // Constructor para inyectar repositorios
        public ExcelProcessingService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
            // Registrar codificación para evitar errores de lectura de caracteres
            System.Text.Encoding.RegisterProvider(System.Text.CodePages.CodePagesEncodingProvider.Instance);
        }

        public async Task<int> ProcessEmployeeData(Stream fileStream)
        {
            int recordsProcessed = 0;
            
            // Usar 'true' para indicar que es un archivo .xlsx (OpenXml)
            using var reader = ExcelReaderFactory.CreateReader(fileStream);

            // Leer los datos del Excel en un DataSet
            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true // La primera fila son los nombres de las columnas
                }
            });

            // Asumimos que la hoja de cálculo se llama "Empleados" o es la primera
            var dataTable = dataSet.Tables[0]; 

            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                // Mapeo básico: Obtener datos por el nombre de la columna
                var email = row["CorreoEmpresarial"].ToString();
                var documento = row["DocumentoIdentidad"].ToString();
                
                // 1. Omitir si ya existe
                if (await _empleadoRepository.ExistsByEmailOrDocumentoAsync(email, documento))
                {
                    continue; 
                }

                // 2. Mapeo Manual (Tendrías que expandir esto para todas las propiedades)
                var nuevoEmpleado = new Empleado
                {
                    Nombres = row["Nombres"].ToString() ?? "N/A",
                    Apellidos = row["Apellidos"].ToString() ?? "N/A",
                    CorreoEmpresarial = email,
                    DocumentoIdentidad = documento,
                    Estado = EstadoEmpleado.Activo,
                    // **Importante:** Las contraseñas deben ser generadas o requeridas. Aquí se pone un hash temporal.
                    PasswordHash = "TEMPORAL_HASH", 
                    
                    // Asegúrate de que los campos numéricos (IDs) puedan convertirse de string a int
                    CargoId = int.Parse(row["CargoId"].ToString() ?? "0"), 
                    DepartamentoId = int.Parse(row["DepartamentoId"].ToString() ?? "0"),
                    NivelEducativoId = int.Parse(row["NivelEducativoId"].ToString() ?? "0"),
                    
                    // Asignación de valores predeterminados requeridos para que Entity Framework no falle
                    Direccion = "Pendiente",
                    Ciudad = "Pendiente",
                    Telefono = "Pendiente",
                    CorreoPersonal = string.Empty,
                    PerfilProfesional = string.Empty,
                    FechaNacimiento = DateTime.UtcNow 
                };
                
                // 3. Guardar
                await _empleadoRepository.AddAsync(nuevoEmpleado);
                recordsProcessed++;
            }

            return recordsProcessed;
        }
    }
}