using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public class CursoAppService : ICursoAppService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly ICursoService _cursoService; 
        private readonly IMapper _mapper;

        public CursoAppService(ICursoRepository cursoRepository, ICursoService cursoService, IMapper mapper)
        {
            _cursoRepository = cursoRepository;
            _cursoService = cursoService;
            _mapper = mapper;
        }

        public async Task<CursoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<CursoViewModel>(await _cursoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<CursoViewModel>> ObterTodos()
        {
           return _mapper.Map<IEnumerable<CursoViewModel>>(await _cursoRepository.ObterTodos());
        }

        public async void Adicionar(CursoViewModel cursoViewModel)
        {
           var _curso = _mapper.Map<Curso>(cursoViewModel);
           _cursoRepository.Adicionar(_curso);

           await _cursoRepository.UnitOfWork.Commit();
        }

        public async void Atualizar(CursoViewModel cursoViewModel)
        {
            var _curso = _mapper.Map<Curso>(cursoViewModel);
            _cursoRepository.Atualizar(_curso);

            await _cursoRepository.UnitOfWork.Commit();
        }


        public async Task<bool> Ativar(Guid id)
        {
            var _sucess = await _cursoService.Ativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Curso");
            }

            return true;
        }

        public async Task<bool> Inativar(Guid id)
        {
            var _sucess = await _cursoService.Inativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Curso");
            }

            return true;
        }


        public void Dispose()
        {
            _cursoRepository?.Dispose();
            _cursoService?.Dispose();
        }

    }
}
