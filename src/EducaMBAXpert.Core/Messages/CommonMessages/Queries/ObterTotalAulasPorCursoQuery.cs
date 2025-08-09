using MediatR;

namespace EducaMBAXpert.Core.Messages.CommonMessages.Queries
{
    public class ObterTotalAulasPorCursoQuery : IRequest<Int32>
    {
        public ObterTotalAulasPorCursoQuery(Guid cursoId)
        {
            CursoId = cursoId;
        }

        public Guid CursoId { get; }
    }
}
