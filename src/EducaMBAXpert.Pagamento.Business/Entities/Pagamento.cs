using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Pagamentos.Business.Entities
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid CobrancaCursoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public Transacao Transacao { get; set; }
    }
}
