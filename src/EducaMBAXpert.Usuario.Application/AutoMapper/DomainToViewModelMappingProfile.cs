using AutoMapper;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Usuario, UsuarioViewModel>()
                 .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos));

            CreateMap<Endereco, EnderecoViewModel>();

            CreateMap<Matricula, MatriculaViewModel>();
        }

    }
}
