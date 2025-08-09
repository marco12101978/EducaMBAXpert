using AutoMapper;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;

namespace EducaMBAXpert.Alunos.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Aluno, AlunoViewModel>()
                 .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos));

            CreateMap<Endereco, EnderecoViewModel>();

            CreateMap<Matricula, MatriculaViewModel>();

            CreateMap<AulaConcluida, AulaConcluidaViewModel>();
        }

    }
}
