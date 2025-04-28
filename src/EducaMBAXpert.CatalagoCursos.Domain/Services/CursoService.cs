using EducaMBAXpert.CatalagoCursos.Domain.Events;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Bus;

namespace EducaMBAXpert.CatalagoCursos.Domain.Services
{
    public class CursoService : ICursoService
    {
        private readonly IMediatrHandler _bus;
        private readonly ICursoRepository _cursoRepository;


        public CursoService(IMediatrHandler bus,ICursoRepository cursoRepository)
        {
            _bus = bus;
            _cursoRepository = cursoRepository;
        }

        public async Task<bool> Ativar(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);

            if (curso == null) return false;

            if (curso.Ativo == true) return false;

            curso.Ativar();

            _cursoRepository.Atualizar(curso);
            var _sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new CursoInativarEvent(curso.Id));
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Inativar(Guid cursoId)
        {
            var curso = await _cursoRepository.ObterPorId(cursoId);

            if (curso == null) return false;

            if (curso.Ativo == false) return false;

            curso.Inativar();

            _cursoRepository.Atualizar(curso);
            var _sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new CursoInativarEvent(curso.Id));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            _cursoRepository.Dispose();
        }
    }
}
