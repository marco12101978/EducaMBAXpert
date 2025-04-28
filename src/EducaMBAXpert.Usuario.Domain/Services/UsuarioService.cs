using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Usuarios.Domain.Events;
using EducaMBAXpert.Usuarios.Domain.Interfaces;

namespace EducaMBAXpert.Usuarios.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMediatrHandler _bus;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IMediatrHandler bus, IUsuarioRepository usuarioRepository)
        {
            _bus = bus;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> Ativar(Guid cursoId)
        {
            var usuario = await _usuarioRepository.ObterPorId(cursoId);

            if (usuario == null) return false;

            if (usuario.Ativo == true) return false;

            usuario.Ativar();

            _usuarioRepository.Atualizar(usuario);
            var _sucesso = await _usuarioRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new UsuarioInativarEvent(usuario.Id));
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Inativar(Guid cursoId)
        {
            var usuario = await _usuarioRepository.ObterPorId(cursoId);

            if (usuario == null) return false;

            if (usuario.Ativo == false) return false;

            usuario.Inativar();

            _usuarioRepository.Atualizar(usuario);
            var _sucesso = await _usuarioRepository.UnitOfWork.Commit();

            if (_sucesso)
            {
                await _bus.PublicarEvento(new UsuarioInativarEvent(usuario.Id));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            _usuarioRepository?.Dispose();
        }
    }
}
