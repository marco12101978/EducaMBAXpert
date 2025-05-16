using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Alunos.Domain.Entities;

namespace EducaMBAXpert.Alunos.Domain.Interfaces
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task<IEnumerable<Aluno>> ObterTodos();
        Task<Aluno> ObterPorId(Guid id);
        Task<Matricula> ObterMatriculaPorId(Guid matriculaId);

        void Adicionar(Aluno aluno);
        void Atualizar(Aluno aluno);
        void AdicionarEndereco(Endereco endereco);

        void AdicionarMatricula(Matricula matricula);
        void AtualizarMatricula(Matricula matricula);
        Task<IEnumerable<Matricula>> ObterTodasMatriculasPorAlunoId(Guid id, bool ativas);

    }
}
