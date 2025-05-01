using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Contracts.Cursos
{
    public interface ICursoConsultaService
    {
        Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId);
        Task<Result<string>> ObterNomeCurso(Guid cursoId);
        Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId,Guid aulaId);
    }
}
