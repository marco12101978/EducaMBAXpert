using EducaMBAXpert.Api.Notificacoes;

namespace EducaMBAXpert.Api.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
