using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioInativarEvent : DomainEvent
    {
        public UsuarioInativarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }

}
