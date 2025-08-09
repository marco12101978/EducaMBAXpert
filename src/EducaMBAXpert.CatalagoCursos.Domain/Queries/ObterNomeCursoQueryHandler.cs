using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Messages.CommonMessages.Queries;
using MediatR;

namespace EducaMBAXpert.CatalagoCursos.Domain.Queries
{
   

    public class ObterNomeCursoQueryHandler : IRequestHandler<ObterNomeCursoQuery, string>
    {
        private readonly ICursoRepository _cursoRepository;

        public ObterNomeCursoQueryHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<string> Handle(ObterNomeCursoQuery request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterNomeCurso(request.CursoId);
            return curso.Data;
        }
    }
}
