using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Application.Interfaces
{
    public interface IEmployeePasswordHasher
    {
        string HashPassword(Empleado employee, string password);
        bool VerifyHashedPassword(Empleado employee, string hashedPassword, string providedPassword);
    }
}