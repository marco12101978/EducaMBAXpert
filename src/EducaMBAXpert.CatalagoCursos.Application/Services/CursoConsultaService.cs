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
            _cursoRepository = cursoRepository ?? throw new ArgumentNullException(nameof(cursoRepository));
        }

        public async Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId,Guid aulaId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<bool>(cursoId);

            var exists = curso.Modulos.SelectMany(m => m.Aulas)
                                      .Any(a => a.Id == aulaId);

            return Result<bool>.Ok(exists);

        }

        public async Task<Result<string>> ObterNomeCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<string>(cursoId);

            return Result<string>.Ok(curso.Titulo);
        }

        public async Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<int>(cursoId);

            var total = curso.Modulos?.Sum(m => m.Aulas?.Count ?? 0) ?? 0;

            return Result<int>.Ok(total);
        }

        private Result<T> CursoNaoEncontrado<T>(Guid cursoId)
        {
            var message = $"Curso com ID {cursoId} não encontrado.";
            return Result<T>.Fail(message);
        }
    }
}
