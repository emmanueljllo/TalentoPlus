using System.Threading.Tasks;

namespace TalentoPlus.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string toEmail, string subject, string message);
    }
}
