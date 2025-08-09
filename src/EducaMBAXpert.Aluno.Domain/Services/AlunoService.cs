using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Alunos.Domain.Events;
using EducaMBAXpert.Alunos.Domain.Interfaces;

namespace EducaMBAXpert.Alunos.Domain.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IMediatrHandler _bus;
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IMediatrHandler bus, IAlunoRepository alunoRepository)
        {
            _bus = bus;
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> Ativar(Guid cursoId)
        {
            var aluno = await _alunoRepository.ObterPorId(cursoId);

            if (aluno == null) return false;

            if (aluno.Ativo == true) return false;

            aluno.Ativar();

            _alunoRepository.Atualizar(aluno);
            var _sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new AlunoInativarEvent(aluno.Id));
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Inativar(Guid cursoId)
        {
            var aluno = await _alunoRepository.ObterPorId(cursoId);

            if (aluno == null) return false;

            if (aluno.Ativo == false) return false;

            aluno.Inativar();

            _alunoRepository.Atualizar(aluno);
            var _sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new AlunoInativarEvent(aluno.Id));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            _alunoRepository?.Dispose();
        }
    }
}
