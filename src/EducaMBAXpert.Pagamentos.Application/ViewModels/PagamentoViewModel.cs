using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Pagamentos.Application.ViewModels
{
    public class PagamentoViewModel
    {
        [Required]
        public Guid CobrancaAnuidadeId { get; set; }

        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O status deve ter no máximo 50 caracteres.")]
        public string Status { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome do cartão deve ter no máximo 100 caracteres.")]
        public string NomeCartao { get; set; }

        [Required]
        [CreditCard(ErrorMessage = "O número do cartão é inválido.")]
        public string NumeroCartao { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "A data de expiração deve estar no formato MM/AA.")]
        public string ExpiracaoCartao { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "O CVV deve ter 3 ou 4 dígitos.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "O CVV deve conter apenas números, com 3 ou 4 dígitos.")]
        public string CvvCartao { get; set; }

        public TransacaoViewModel Transacao { get; set; }
    }
}
