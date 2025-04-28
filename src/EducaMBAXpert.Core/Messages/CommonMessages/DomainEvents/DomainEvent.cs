using EducaMBAXpert.Core.Messages;

namespace EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggredateId)
        {
            AggregateID = aggredateId;
        }
    }
}
