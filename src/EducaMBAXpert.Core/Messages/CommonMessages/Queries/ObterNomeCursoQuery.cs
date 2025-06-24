using MediatR;

namespace EducaMBAXpert.Core.Messages.CommonMessages.Queries
{
    public class ObterNomeCursoQuery : IRequest<string>
    {
        public Guid CursoId { get; }

        public ObterNomeCursoQuery(Guid cursoId)
        {
            CursoId = cursoId;
        }
    }
}
