using EducaMBAXpert.Alunos.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.Alunos.Domain.Events
{
    public class AlunoEventHandler : INotificationHandler<AlunoInativarEvent>
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoEventHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task Handle(AlunoInativarEvent mensagem, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(mensagem.AggregateID);
        }
    }

}
