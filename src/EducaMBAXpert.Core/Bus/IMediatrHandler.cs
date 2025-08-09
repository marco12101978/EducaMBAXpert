using EducaMBAXpert.Core.Messages;
using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace EducaMBAXpert.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
        Task<TResult> EnviarQuery<TQuery, TResult>(TQuery query) where TQuery : IRequest<TResult>;
    }
}
