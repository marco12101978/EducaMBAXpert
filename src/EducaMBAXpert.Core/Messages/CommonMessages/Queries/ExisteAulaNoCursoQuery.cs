using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducaMBAXpert.Core.Messages.CommonMessages.Queries
{
    public class ExisteAulaNoCursoQuery : IRequest<bool>
    {
        public ExisteAulaNoCursoQuery(Guid cursoId, Guid aulaId)
        {
            CursoId = cursoId;
            AulaId = aulaId;
        }

        public Guid CursoId { get; }
        public Guid AulaId { get; }


    }
}
