using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducaMBAXpert.Pagamentos.Business.Entities;
using EducaMBAXpert.Pagamentos.Business.Models;

namespace EducaMBAXpert.Pagamentos.Business.Interfaces
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(CobrancaCurso cobrancaCurso, Pagamento pagamento);
    }
}
