using EducaMBAXpert.Contracts.Cursos;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    internal class CursoConsultaAppService : ICursoConsultaService
    {
        public Task<string> ObterNomeCurso(Guid cursoId)
        {
            throw new NotImplementedException();
        }

        public Task<int> ObterTotalAulasPorCurso(Guid cursoId)
        {
            throw new NotImplementedException();
        }
    }
}
