﻿using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Pagamentos.Application.ViewModels
{
    public class PagamentoViewModel
    {
        public Guid Id { get; set; }
        public Guid CobrancaCursoId { get; set; }
        public Guid AlunoId { get; set; }

        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; } = string.Empty;
        public string NumeroCartao { get; set; } = string.Empty;
        public string ExpiracaoCartao { get; set; } = string.Empty;
        public string CvvCartao { get; set; } = string.Empty;

        public TransacaoViewModel Transacao { get; set; } = new TransacaoViewModel();
    }
}
