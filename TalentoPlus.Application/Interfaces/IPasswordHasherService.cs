
namespace TalentoPlus.Application.Interfaces
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
        bool Verify(string hashed, string provided);
    }
}
