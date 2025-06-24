using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Messages.CommonMessages.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducaMBAXpert.CatalagoCursos.Domain.Queries
{

    public class ObterTotalAulasPorCursoQueryHandler : IRequestHandler<ObterTotalAulasPorCursoQuery, Int32>
    {
        private readonly ICursoRepository _cursoRepository;

        public ObterTotalAulasPorCursoQueryHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Int32> Handle(ObterTotalAulasPorCursoQuery request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterTotalAulasPorCurso(request.CursoId);
            return curso.Data;
        }
    }
}
