using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "A rua é obrigatória.")]
        [StringLength(100, ErrorMessage = "A rua deve ter no máximo 100 caracteres.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
        public string Numero { get; set; }

        [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O bairro deve ter no máximo 100 caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Informe a sigla do estado (UF).")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Formato do CEP inválido. Ex: 00000-000")]
        public string Cep { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
