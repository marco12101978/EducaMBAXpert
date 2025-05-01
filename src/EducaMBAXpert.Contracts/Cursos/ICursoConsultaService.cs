namespace EducaMBAXpert.Contracts.Cursos
{
    public interface ICursoConsultaService
    {
        Task<int> ObterTotalAulasPorCurso(Guid cursoId);
        Task<string> ObterNomeCurso(Guid cursoId);
        Task<bool> ExiteAulaNoCurso(Guid cursoId,Guid aulaId);


    }
}
