using AutoMapper;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmpleadoRegisterDto, Empleado>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CorreoEmpresarial)) 
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CorreoEmpresarial));   
        }
    }
}