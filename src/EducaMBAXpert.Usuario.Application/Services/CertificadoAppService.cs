using EducaMBAXpert.Contracts.Cursos;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    public class CertificadoAppService : ICertificadoAppService
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
