
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Events
{
    public class CursoAtivarEvent : DomainEvent
    {
        public CursoAtivarEvent(Guid aggredateId) : base(aggredateId)
        {

        }
    }
}
