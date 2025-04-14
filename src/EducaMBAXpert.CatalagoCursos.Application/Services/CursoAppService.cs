using AutoMapper;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public class CursoAppService : ICursoAppService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public CursoAppService(ICursoRepository cursoRepository, IMapper mapper)
        {
            _cursoRepository = cursoRepository;
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

        public async Task<bool> Inativar(Guid id)
        {
            return true;
        }

        public async Task<bool> Ativar(Guid id)
        {
            return true;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
