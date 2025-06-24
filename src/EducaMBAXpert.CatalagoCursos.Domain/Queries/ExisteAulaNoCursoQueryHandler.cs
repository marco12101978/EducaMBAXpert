using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Messages.CommonMessages.Queries;
using MediatR;

namespace EducaMBAXpert.CatalagoCursos.Domain.Queries
{
    public class ExisteAulaNoCursoQueryHandler : IRequestHandler<ExisteAulaNoCursoQuery, bool>
    {
        private readonly ICursoRepository _cursoRepository;

        public ExisteAulaNoCursoQueryHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<bool> Handle(ExisteAulaNoCursoQuery request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ExisteAulaNoCurso(request.CursoId,request.AulaId);
            return curso.Data;
        }
    }
}
