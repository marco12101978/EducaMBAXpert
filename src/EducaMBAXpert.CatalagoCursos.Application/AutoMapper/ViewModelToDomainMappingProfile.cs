using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;

namespace EducaMBAXpert.CatalagoCursos.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CursoViewModel, Curso>();
        }
    }
}
