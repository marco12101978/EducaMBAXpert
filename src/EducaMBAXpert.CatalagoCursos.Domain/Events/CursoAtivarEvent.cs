using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.CatalagoCursos.Domain.Events
{
    public class CursoAtivarEvent : DomainEvent
    {
        public CursoAtivarEvent(Guid aggredateId) : base(aggredateId)
        {

        }
    }
}
