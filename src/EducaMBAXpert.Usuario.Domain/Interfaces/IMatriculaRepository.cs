using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Alunos.Domain.Entities;

namespace EducaMBAXpert.Alunos.Domain.Interfaces
{
    public interface IMatriculaRepository : IRepository<Entities.Matricula>
    {
        Task<Matricula> ObterPorIdAsync(Guid id);
        Task Atualizar(Matricula matricula);

        Task<bool> AulaJaConcluida(Guid matriculaId, Guid aulaId);
        Task AdicionarAulaConcluida(AulaConcluida aulaConcluida);
    }
}

