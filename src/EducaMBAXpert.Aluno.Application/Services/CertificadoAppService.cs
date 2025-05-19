using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Core.Data;

namespace EducaMBAXpert.Alunos.Application.Services
{
    public class CertificadoAppService : ICertificadoConsultaAppService
    {
        private readonly ICursoConsultaService _cursoConsulta;

        public CertificadoAppService(ICursoConsultaService cursoConsulta)
        {
            _cursoConsulta = cursoConsulta;
        }

        public async Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId)
        {
            var totalAulas = await _cursoConsulta.ObterTotalAulasPorCurso(cursoId);


            return false;
        }
    }
}
