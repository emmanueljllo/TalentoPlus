using TalentoPlus.Domain.Repositories;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Enums;
using AutoMapper;
using TalentoPlus.Application.Exceptions;
using System.Threading.Tasks;

namespace TalentoPlus.Application.Handlers
{
    public class AutoregistroCommandHandler
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IEmployeePasswordHasher _passwordHasher;

        public AutoregistroCommandHandler(
            IEmpleadoRepository empleadoRepository,
            IEmailService emailService,
            IMapper mapper,
            IEmployeePasswordHasher passwordHasher)
        {
            _empleadoRepository = empleadoRepository;
            _emailService = emailService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(EmpleadoRegisterDto dto)
        {
            // 1. Verificar unicidad de Correo y Documento
            var exists = await _empleadoRepository.ExistsByEmailOrDocumentoAsync(dto.CorreoEmpresarial, dto.DocumentoIdentidad);
            if (exists)
            {
                throw new ConflictException("Ya existe un empleado con ese correo o documento de identidad.");
            }

            // 2. Mapeo DTO a Entidad
            var nuevoEmpleado = _mapper.Map<Empleado>(dto);
            nuevoEmpleado.Estado = EstadoEmpleado.Activo; // Asignar estado inicial

            // 3. Hashing de Contraseña (Usando la interfaz)
            nuevoEmpleado.PasswordHash = _passwordHasher.HashPassword(nuevoEmpleado, dto.Password);

            // 4. Guardar en repositorio
            await _empleadoRepository.AddAsync(nuevoEmpleado);

            // 5. Enviar Correo (Opcional)
            var subject = "Registro Exitoso en TalentoPlus";
            var content = $"Bienvenido, {nuevoEmpleado.Nombres}. Tu cuenta ha sido creada con éxito.";
            await _emailService.SendEmailAsync(nuevoEmpleado.CorreoEmpresarial, subject, content);
        }
    }
}