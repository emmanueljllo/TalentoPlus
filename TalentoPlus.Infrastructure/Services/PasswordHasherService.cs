using Microsoft.AspNetCore.Identity;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();

        public string Hash(string password) => _hasher.HashPassword(null, password);

        public bool Verify(string hashed, string provided)
        {
            return _hasher.VerifyHashedPassword(null, hashed, provided) == PasswordVerificationResult.Success;
        }
    }
}