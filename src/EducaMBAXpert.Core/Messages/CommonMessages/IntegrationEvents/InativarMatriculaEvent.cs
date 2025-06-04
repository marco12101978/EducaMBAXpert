namespace EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents
{
    public class InativarMatriculaEvent : IntegrationEvent
    {
        public InativarMatriculaEvent(Guid id)
        {
            AggregateID = id;
        }
    }
}
