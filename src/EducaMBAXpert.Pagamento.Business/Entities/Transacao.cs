using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Pagamentos.Business.Enum;

namespace EducaMBAXpert.Pagamentos.Business.Entities
{
    public class Transacao : Entity
    {
        public Guid CobrancaAnuidadeId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao StatusTransacao { get; set; }

        // EF. Rel.
        public Pagamento Pagamento { get; set; }
    }
}
