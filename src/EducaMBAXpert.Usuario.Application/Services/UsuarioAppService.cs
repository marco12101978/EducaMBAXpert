using AutoMapper;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Usuarios.Application.Interfaces;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;
using EducaMBAXpert.Usuarios.Domain.Interfaces;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioAppService(IUsuarioRepository usuarioRepository,
                                 IUsuarioService usuarioService,
                                 IMapper mapper)
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


        public async Task Adicionar(UsuarioViewModel usuarioViewModel)
        {
            var _usuario = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Adicionar(_usuario);

            await _usuarioRepository.UnitOfWork.Commit();
        }

        public async Task Atualizar(UsuarioViewModel usuarioViewModel)
        {
            var _usuario = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Atualizar(_usuario);

            await _usuarioRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Ativar(Guid id)
        {
            var _sucess = await _usuarioService.Ativar(id);
            if (!_sucess)
            {
                throw new DomainException("Falha ao Inativar Usuario");
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

        public async Task AdicionarEndereco(EnderecoViewModel enderecoViewModel)
        {
            var _endereco = _mapper.Map<Endereco>(enderecoViewModel);
            _usuarioRepository.AdicionarEndereco(_endereco);

            await _usuarioRepository.UnitOfWork.Commit();
        }

        public async Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel)
        {
            var _matricula = _mapper.Map<Matricula>(matriculaInputModel);

            _usuarioRepository.AdicionarMatricula(_matricula);

            await _usuarioRepository.UnitOfWork.Commit();
        }


        public async Task<MatriculaViewModel> ObterMatriculaPorId(Guid id)
        {
            return _mapper.Map<MatriculaViewModel>(await _usuarioRepository.ObterMatriculaPorId(id));
        }

        public async Task<IEnumerable<MatriculaViewModel>> ObterTodasMatriculasPorUsuarioId(Guid id,bool ativas)
        {
            return _mapper.Map<IEnumerable<MatriculaViewModel>>(await _usuarioRepository.ObterTodasMatriculasPorUsuarioId(id, ativas));
        }


        public void Dispose()
        {
            _usuarioRepository?.Dispose();
            _usuarioService?.Dispose();
        }


    }
}
