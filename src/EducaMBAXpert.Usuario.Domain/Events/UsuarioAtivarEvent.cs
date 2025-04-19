using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioAtivarEvent : DomainEvent
    {
        public UsuarioAtivarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }
}
