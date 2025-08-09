namespace EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents
{
    public class AtivarMatriculaEvent : IntegrationEvent
    {
        public AtivarMatriculaEvent(Guid id)
        {
            AggregateID = id;
        }
    }
}
