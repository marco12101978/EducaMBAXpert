using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducaMBAXpert.Core.Messages.CommonMessages.Notifications
{
    public class NotificationContext
    {
        private readonly DomainNotificationHandler _domainNotificationHandler;

        public NotificationContext(DomainNotificationHandler domainNotificationHandler)
        {
            _domainNotificationHandler = domainNotificationHandler;
        }

        public bool TemNotificacao() => _domainNotificationHandler.TemNotificacao();
        public List<DomainNotification> ObterNotificacoes() => _domainNotificationHandler.ObterNotificacoes();
    }
}
