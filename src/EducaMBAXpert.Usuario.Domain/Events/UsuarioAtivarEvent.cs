using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioAtivarEvent : DomainEvent
    {
        public UsuarioAtivarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }
}
