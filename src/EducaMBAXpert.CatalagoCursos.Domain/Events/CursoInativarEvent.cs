using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.CatalagoCursos.Domain.Events
{
    public class CursoInativarEvent : DomainEvent
    {
        public CursoInativarEvent(Guid aggredateId) : base(aggredateId)
        {

        }
    }
}
