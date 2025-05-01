using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Core.Messages;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public class CursoAppService : ICursoComandoAppService, ICursoConsultaAppService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly ICursoService _cursoService;
        private readonly IMapper _mapper;
        private readonly IMediatrHandler _mediatrHandler;

        public CursoAppService(ICursoRepository cursoRepository,
                               ICursoService cursoService,
                               IMapper mapper,
                               IMediatrHandler mediatrHandler)
        {
            _cursoRepository = cursoRepository;
            _cursoService = cursoService;
            _mapper = mapper;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<CursoViewModel?> ObterPorId(Guid id)
        {
            Curso? curso = await _cursoRepository.ObterPorId(id);
            if (curso == null)
            {
                await NotificarErro("ObterPorId", $"Curso com ID {id} não encontrado.");
                return null;
            }

            return _mapper.Map<CursoViewModel>(curso);
        }

        public async Task<IEnumerable<CursoViewModel>> ObterTodos()
        {
            var cursos = await _cursoRepository.ObterTodos();
            return _mapper.Map<IEnumerable<CursoViewModel>>(cursos);
        }


        public async Task<Result> Adicionar(CursoInputModel inputModel)
        {
            if (inputModel == null)
            {
                var message = $"O parâmetro {nameof(inputModel)} não pode ser nulo.";
                return await NotificarErro("CursoAppService.Adicionar", message);
            }

            var curso = new Curso(inputModel.Titulo,
                                   inputModel.Descricao,
                                   inputModel.Valor,
                                   inputModel.Categoria,
                                   inputModel.Nivel);

            if (inputModel.Ativo)
                curso.Ativar();
            else
                curso.Inativar();


            if (inputModel.Modulos == null || !inputModel.Modulos.Any())
            {
                var message = "O curso precisa ter pelo menos um módulo.";
                return await NotificarErro("CursoAppService.Adicionar", message);
            }


            foreach (var moduloVm in inputModel.Modulos)
            {
                if (moduloVm.Aulas == null || !moduloVm.Aulas.Any())
                    continue;

                var modulo = new Modulo(moduloVm.Nome);

                foreach (var aulaVm in moduloVm.Aulas)
                {
                    var aula = new Aula(aulaVm.Titulo, aulaVm.Url, aulaVm.Duracao);
                    modulo.AdicionarAula(aula);
                }

                curso.AdicionarModulo(modulo);
            }

            _cursoRepository.Adicionar(curso);

            await _cursoRepository.UnitOfWork.Commit();

            return Result.Ok();
        }

        public async Task<Result> Atualizar(CursoInputModel inputModel)
        {
            if (inputModel == null)
            {
                var message = $"O parâmetro {nameof(inputModel)} não pode ser nulo.";
                return await NotificarErro("CursoAppService.Atualizar", message);
            }

            var curso = _mapper.Map<Curso>(inputModel);
            _cursoRepository.Atualizar(curso);

            await _cursoRepository.UnitOfWork.Commit();
            return Result.Ok();
        }


        public async Task<Result> Ativar(Guid id)
        {
            var success = await _cursoService.Ativar(id);
            if (!success)
            {
                var message = $"Falha ao ativar curso com ID {id}.";
                return await NotificarErro("CursoAppService.Ativar", message);
            }

            return Result.Ok();
        }

        public async Task<Result> Inativar(Guid id)
        {
            var success = await _cursoService.Inativar(id);
            if (!success)
            {
                var message = $"Falha ao inativar curso com ID {id}.";
                return await NotificarErro("CursoAppService.Inativar", message);
            }

            return Result.Ok();
        }


        public void Dispose()
        {
            _cursoRepository?.Dispose();
            _cursoService?.Dispose();
        }


        private async Task<Result> NotificarErro(string contexto, string mensagem)
        {
            await _mediatrHandler.PublicarNotificacao(new DomainNotification(contexto, mensagem));
            return Result.Fail(mensagem);
        }


    }
}
