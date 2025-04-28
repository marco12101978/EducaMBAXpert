using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Pagamentos.Application.ViewModels
{
    public class TransacaoViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "O código de autorização deve ter no máximo 20 caracteres.")]
        public string CodigoAutorizacao { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "A bandeira do cartão deve ter no máximo 30 caracteres.")]
        public string BandeiraCartao { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O status da transação deve ter no máximo 50 caracteres.")]
        public string StatusTransacao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
        public decimal ValorTotal { get; set; }
    }
}
