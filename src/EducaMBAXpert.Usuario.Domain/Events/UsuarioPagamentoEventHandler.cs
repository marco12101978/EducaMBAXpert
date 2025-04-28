using EducaMBAXpert.Usuarios.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioEventHandler : INotificationHandler<UsuarioInativarEvent>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioEventHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task Handle(UsuarioInativarEvent mensagem, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorId(mensagem.AggregateID);
        }
    }

}
