using EducaMBAXpert.Usuarios.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class UsuarioInputModel
    {
        public UsuarioInputModel(Guid id, string nome, string email, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Ativo = ativo;
        }

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail está em formato inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
        public string Email { get; set; }

        public bool Ativo { get; set; }

        // Relacionamentos
        public List<EnderecoViewModel> Enderecos { get; set; } = new();

        public List<Matricula> Matriculas { get; set; } = new();
    }
}
