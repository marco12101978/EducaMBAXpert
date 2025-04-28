using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;

namespace EducaMBAXpert.CatalagoCursos.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Curso, CursoViewModel>();
            CreateMap<Modulo, ModuloViewModel>();
            CreateMap<Aula, AulaViewModel>();
        }
    }
}
