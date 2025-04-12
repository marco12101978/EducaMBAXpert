using EducaMBAXpert.Core.Messages;

namespace EducaMBAXpert.Core.DomainObjects
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggredateId)
        {
            AggregateID = aggredateId;
        }
    }
}
