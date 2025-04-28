using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.CatalagoCursos.Domain.Events
{
    public class CursoEventHandler : INotificationHandler<CursoInativarEvent>
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoEventHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task Handle(CursoInativarEvent mensagem, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorId(mensagem.AggregateID);

            //enviar emial, sms , etc...

        }
    }
}
