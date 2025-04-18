﻿using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email em formato inválido.")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres.")]
        public string Email { get; set; }

        public bool Ativo { get; set; }

        public List<EnderecoViewModel> Enderecos { get; set; } = new();
    }
}
