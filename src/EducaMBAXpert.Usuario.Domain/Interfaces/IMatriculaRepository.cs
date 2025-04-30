using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Domain.Interfaces
{
    public interface IMatriculaRepository : IRepository<Entities.Matricula>
    {
        Task<Matricula> ObterPorIdAsync(Guid id);
        Task Atualizar(Matricula matricula);
    }
}

