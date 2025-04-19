using AutoMapper;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;
using EducaMBAXpert.Usuarios.Domain.Interfaces;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    internal class UsuarioAppService : IUsuarioAppService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioAppService(IUsuarioRepository usuarioRepository, IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }


        public async Task<UsuarioViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<UsuarioViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(await _usuarioRepository.ObterTodos());
        }


        public async void Adicionar(UsuarioViewModel usuarioViewModel)
        {
            var _curso = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Adicionar(_curso);

            await _usuarioRepository.UnitOfWork.Commit();
        }



        public async void Atualizar(UsuarioViewModel usuarioViewModel)
        {
            var _curso = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Atualizar(_curso);

            await _usuarioRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Ativar(Guid id)
        {
            var _sucess = await _usuarioService.Ativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Curso");
            }

            return true;
        }

        public  async Task<bool> Inativar(Guid id)
        {
            var _sucess = await _usuarioService.Inativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Usuario");
            }

            return true;
        }




        public void Dispose()
        {
            _usuarioRepository?.Dispose();
            _usuarioService?.Dispose();
        }
    }
}
