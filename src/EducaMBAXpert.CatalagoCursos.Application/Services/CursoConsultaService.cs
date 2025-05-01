using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public class CursoConsultaService : ICursoConsultaService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoConsultaService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId,Guid aulaId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);

            if (curso == null)
                return CursoNaoEncontrado<bool>();

            var exists = curso.Modulos.SelectMany(m => m.Aulas)
                                      .Any(a => a.Id == aulaId);

            return Result<bool>.Ok(exists);

        }

        public async Task<Result<string>> ObterNomeCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<string>();

            return Result<string>.Ok(curso.Titulo);
        }

        public async Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<int>();

            var total = curso.Modulos.SelectMany(m => m.Aulas).Count();
            return Result<int>.Ok(total);
        }

        private Result<T> CursoNaoEncontrado<T>()
        {
            return Result<T>.Fail("Curso não encontrado.");
        }
    }
}
