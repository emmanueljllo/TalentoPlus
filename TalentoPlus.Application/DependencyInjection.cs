using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TalentoPlus.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registra AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Registra todos los Handlers (Commands/Queries)
            services.AddScoped<Handlers.LoginCommandHandler>();
            services.AddScoped<Handlers.AutoregistroCommandHandler>();
            // ... Registrar el resto de Handlers aquí ...

            return services;
        }
    }
}