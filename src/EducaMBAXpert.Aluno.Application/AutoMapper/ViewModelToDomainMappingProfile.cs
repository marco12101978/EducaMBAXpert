using AutoMapper;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;

namespace EducaMBAXpert.Alunos.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AlunoInputModel, Aluno>()
                .ConstructUsing(u => new Aluno(u.Id, u.Nome, u.Email,u.Ativo));

            CreateMap<EnderecoInputModel, Endereco>()
                .ConstructUsing(e =>
                    new Endereco(e.Rua, e.Numero, e.Complemento, e.Bairro, e.Cidade, e.Estado, e.Cep, e.AlunoId)
                );

            CreateMap<MatriculaInputModel, Matricula>()
                .ConstructUsing(m => new Matricula(m.AlunoId, m.CursoId));
        }
    }
}
