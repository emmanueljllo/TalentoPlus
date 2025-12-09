using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TalentoPlus.Application; 
using TalentoPlus.Infrastructure; 
using TalentoPlus.Infrastructure.Persistence;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 1. CONFIGURACIÓN DE BASE DE DATOS (MySQL)
// ====================================================================
var connectionString = builder.Configuration.GetConnectionString("MySQLDB");

// Usamos AddDbContext para configurar el contexto con MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// ====================================================================
// 2. CONFIGURACIÓN DE IDENTITY Y AUTENTICACIÓN
// ====================================================================

// Añadir el servicio de Identity para gestionar usuarios y roles (IdentityUser)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Opciones de seguridad de contraseña (ajustar según requisitos)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Usa nuestro contexto para las tablas Identity
.AddDefaultTokenProviders(); // Necesario para SignInManager, Password Resets, etc.


// Configuración de JWT Bearer
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        // Usamos Encoding.UTF8.GetBytes para convertir la clave secreta a bytes
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT Key not configured")))
    };
});

// ====================================================================
// 3. INYECCIÓN DE DEPENDENCIAS (IoC)
// ====================================================================

// Llama a los métodos de extensión para registrar servicios de Application e Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// ====================================================================
// 4. CONFIGURACIÓN DE MVC y SWAGGER
// ====================================================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TalentoPlus API", Version = "v1" });
    
    // Configuración para usar JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token JWT en este formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// ====================================================================
// 5. PIPELINE DE SOLICITUDES (Middleware y Seeding)
// ====================================================================

var app = builder.Build();

// --- Ejecución de Migraciones y Seeding ---
// Este código solo corre en Desarrollo para configurar la DB automáticamente
if (app.Environment.IsDevelopment())
{
    // Aplicar Migraciones
    await app.ApplyMigrations(); 
    
    // Ejecución del Seeder para crear el Admin inicial y roles
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Crea Roles y el usuario: admin@talentoplus.com / Admin123*
        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
        Console.WriteLine("Usuario Administrador y Roles iniciales creados.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar los datos iniciales.");
    }
}

// Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 

// Orden Crucial: UseAuthentication debe ir antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();