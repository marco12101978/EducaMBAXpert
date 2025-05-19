using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Core.Data
{
    public interface ICursoConsultaService
    {
        Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId);
        Task<Result<string>> ObterNomeCurso(Guid cursoId);
        Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId, Guid aulaId);
    }
}
