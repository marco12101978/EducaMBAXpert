using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Usuarios.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.Usuarios.Domain.Events
{
    public class UsuarioPagamentoEventHandler : INotificationHandler<PagamentoRealizadoEvent>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioPagamentoEventHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task Handle(PagamentoRealizadoEvent mensagem, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorId(mensagem.AggregateID);
        }
    }

}
