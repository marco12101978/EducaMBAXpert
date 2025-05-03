using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Usuarios.Application.Interfaces;

namespace EducaMBAXpert.Usuarios.Application.Services
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
