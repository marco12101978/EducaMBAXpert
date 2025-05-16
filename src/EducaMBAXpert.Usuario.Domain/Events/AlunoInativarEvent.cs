using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.Alunos.Domain.Events
{
    public class AlunoInativarEvent : DomainEvent
    {
        public AlunoInativarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }

}
