using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Entities;
using Microsoft.AspNetCore.Identity; 

namespace TalentoPlus.Infrastructure.Services
{
    public class EmployeePasswordHasher : IEmployeePasswordHasher
    {
        private readonly PasswordHasher<Empleado> _hasher = new PasswordHasher<Empleado>();

        public string HashPassword(Empleado employee, string password)
        {
            // Identity necesita una instancia de la clase genérica para hashear
            return _hasher.HashPassword(employee, password);
        }

        public bool VerifyHashedPassword(Empleado employee, string hashedPassword, string providedPassword)
        {
            return _hasher.VerifyHashedPassword(employee, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
        }
    }
}