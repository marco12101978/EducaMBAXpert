using EducaMBAXpert.Core.Messages;
using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace EducaMBAXpert.Core.Bus
{
    public class MediatorHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }

        public async Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
        {
            await _mediator.Publish(notificacao);
        }

        public Task<TResult> EnviarQuery<TQuery, TResult>(TQuery query) where TQuery : IRequest<TResult>
        {
            return _mediator.Send(query);
        }
    }
}
