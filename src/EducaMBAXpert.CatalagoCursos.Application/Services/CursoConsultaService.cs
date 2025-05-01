using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Contracts.Cursos;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public class CursoConsultaService : ICursoConsultaService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoConsultaService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<bool> ExiteAulaNoCurso(Guid cursoId,Guid aulaId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);

            return curso.Modulos.SelectMany(m => m.Aulas)
                                .Any(a => a.Id == aulaId);

        }

        public async Task<string> ObterNomeCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            return curso?.Titulo ?? "Curso não encontrado";
        }

        public async Task<int> ObterTotalAulasPorCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            return curso?.Modulos.SelectMany(m => m.Aulas).Count() ?? 0;
        }
    }
}
