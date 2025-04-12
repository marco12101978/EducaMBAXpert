
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Events
{
    public class CursoInativarEvent : DomainEvent
    {
        public CursoInativarEvent(Guid aggredateId) : base(aggredateId)
        {

        }
    }
}
