using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/matriculas")]
    [Authorize]
    public class MatriculasController : MainController
    {
        private readonly IAlunoComandoAppService _alunoComandoAppService;
        private readonly IAlunoConsultaAppService _alunoConsultaAppService;
        private readonly IMatriculaConsultaAppService _matriculaConsultaAppService;
        private readonly IMatriculaComandoAppService _matriculaComandoAppService;
        private readonly ICursoConsultaService _cursoConsultaService;
        private readonly ICursoConsultaAppService _cursoConsultaAppService;
        private readonly IMediator _mediator;

        public MatriculasController(IMediator mediator,
                                    IAlunoConsultaAppService alunoConsultaAppService,
                                    IAlunoComandoAppService alunoComandoAppService,
                                    IMatriculaConsultaAppService matriculaConsultaAppService,
                                    IMatriculaComandoAppService matriculaComandoAppService,
                                    ICursoConsultaService cursoConsultaService,
                                    ICursoConsultaAppService cursoConsultaAppService,
                                    NotificationContext notificationContext,
                                    IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _alunoConsultaAppService = alunoConsultaAppService;
            _alunoComandoAppService = alunoComandoAppService;
            _matriculaConsultaAppService = matriculaConsultaAppService;
            _matriculaComandoAppService = matriculaComandoAppService;
            _cursoConsultaService = cursoConsultaService;
            _cursoConsultaAppService = cursoConsultaAppService;
            _mediator = mediator;
        }

        [HttpPost("matricular/{idAluno:guid}")]
        [SwaggerOperation(Summary = "Matricular aluno", Description = "Executa a matrícula no curso.")]
        [ProducesResponseType(typeof(MatriculaInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Matricular(Guid idAluno, [FromBody] MatriculaInputModel matricula)
        {
            if (!ModelState.IsValid || matricula.AlunoId != idAluno)
                return CustomResponse(HttpStatusCode.BadRequest);

            var aluno = await _alunoConsultaAppService.ObterPorId(idAluno);
            if (aluno == null)
                return NotFoundResponse("Aluno não encontrado.");

            var curso = await _cursoConsultaAppService.ObterPorId(matricula.CursoId);
            if (curso == null)
                return NotFoundResponse("Curso não encontrado.");

            await _alunoComandoAppService.AdicionarMatriculaCurso(matricula);
            return CustomResponse(HttpStatusCode.OK);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("aluno/{idAluno:guid}/matriculas-ativas")]
        [SwaggerOperation(Summary = "Listar matrículas ativas", Description = "Retorna as matrículas ativas por usuário.")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterAtivasPorIdAluno(Guid idAluno)
        {
            var matriculas = await _alunoConsultaAppService.ObterTodasMatriculasPorAlunoId(idAluno,true);
            return CustomResponse(HttpStatusCode.OK, matriculas);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("aluno/{idAluno:guid}/matriculas-inativas")]
        [SwaggerOperation(Summary = "Listar matrículas inativas", Description = "Retorna as matrículas inativas por aluno.")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterNaoAtivasPorIdAluno(Guid idAluno)
        {
            var matriculas = await _alunoConsultaAppService.ObterTodasMatriculasPorAlunoId(idAluno, false);
            return CustomResponse(HttpStatusCode.OK, matriculas);
        }


        [HttpPut("aluno/matriculas/{matriculaId}/ativar")]
        [SwaggerOperation(Summary = "Ativar a matrículas", Description = "Ativar a matrícula sem a necessidade de pagamento.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtivarMatricula(Guid matriculaId)
        {
            var _matricula = await _matriculaConsultaAppService.ObterMatricula(matriculaId);
            if (_matricula == null)
                return NotFoundResponse("Matrícula não encontrada.");

            if (_matricula.Ativo == true)
                return NotFoundResponse("Matrícula já se encontra ativa.");


            var ativarMatriculaEvent = new AtivarMatriculaEvent(matriculaId);

            await _mediator.Publish(ativarMatriculaEvent);

            return CustomResponse(HttpStatusCode.OK);

        }

        [HttpPut("aluno/matricula/{matriculaId}/inativar")]
        [SwaggerOperation(Summary = "Inativar a matrículas", Description = "Inativar a matrícula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InativarMatricula(Guid matriculaId)
        {
            var _matricula = await _matriculaConsultaAppService.ObterMatricula(matriculaId);
            if (_matricula == null)
                return NotFoundResponse("Matrícula não encontrada.");

            if (_matricula.Ativo == false)
                return NotFoundResponse("Matrícula já se encontra ativa.");

            var inativarMatriculaEvent = new InativarMatriculaEvent(matriculaId);

            await _mediator.Publish(inativarMatriculaEvent);

            return CustomResponse(HttpStatusCode.OK);

        }


        [HttpPost("matricula/{matriculaId}/aula/{aulaId}/concluir")]
        [SwaggerOperation(Summary = "Concluir aula", Description = "Marca uma aula como concluída para a matrícula.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConcluirAula(Guid matriculaId, Guid aulaId)
        {
            var _matriculas = await _matriculaConsultaAppService.ObterMatricula(matriculaId);
            if (_matriculas == null)
                return NotFoundResponse("Matrícula não encontrada.");

            if (_matriculas.Ativo == false)
                return NotFoundResponse("Matrícula não ativa.");

            var aulaExiste = await _cursoConsultaService.ExisteAulaNoCurso(_matriculas.CursoId, aulaId);
            if (!aulaExiste.Data)
                return NotFoundResponse("Aula não encontrada no curso.");

            await _matriculaComandoAppService.ConcluirAula(matriculaId, aulaId);

            return CustomResponse(HttpStatusCode.OK);
        }


        [HttpGet("matricula/{matriculaId}/certificado")]
        [SwaggerOperation(Summary = "Verificar certificado", Description = "Verifica se o certificado pode ser emitido.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerificarCertificado(Guid matriculaId)
        {
            var podeEmitir = await _matriculaConsultaAppService.PodeEmitirCertificado(matriculaId);
            return CustomResponse(HttpStatusCode.OK, new { podeEmitir });
        }

        [HttpGet("matricula/{matriculaId}/certificado/download")]
        [SwaggerOperation(Summary = "Baixar certificado", Description = "Gera e baixa o certificado em PDF.")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BaixarCertificado(Guid matriculaId)
        {
            var pdf = await _matriculaConsultaAppService.GerarCertificadoPDF(matriculaId);
            if (pdf == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Não foi possível gerar o certificado.");

            return File(pdf, "application/pdf", "Certificado.pdf");
        }


        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
