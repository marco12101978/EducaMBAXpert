using EducaMBAXpert.Core.Messages.CommonMessages.DomainEvents;

namespace EducaMBAXpert.Alunos.Domain.Events
{
    public class AlunoAtivarEvent : DomainEvent
    {
        public AlunoAtivarEvent(Guid aggredateId) : base(aggredateId)
        {
            
        }
    }
}
