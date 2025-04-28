using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioInativarEvent : DomainEvent
    {
        public UsuarioInativarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }

}
