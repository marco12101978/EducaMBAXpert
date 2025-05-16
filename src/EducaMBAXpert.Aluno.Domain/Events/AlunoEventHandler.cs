using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.Alunos.Domain.Events
{
    public class AlunoPagamentoEventHandler : INotificationHandler<PagamentoRealizadoEvent>
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoPagamentoEventHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task Handle(PagamentoRealizadoEvent mensagem, CancellationToken cancellationToken)
        {
            Entities.Matricula matricula = await _alunoRepository.ObterMatriculaPorId(mensagem.AggregateID);

            matricula.Ativar();

            _alunoRepository.AtualizarMatricula(matricula);

            await _alunoRepository.UnitOfWork.Commit();
        }
    }

}
