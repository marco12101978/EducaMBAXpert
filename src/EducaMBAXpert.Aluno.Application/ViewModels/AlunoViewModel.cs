using EducaMBAXpert.Alunos.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Alunos.Application.ViewModels
{
    public class AlunoViewModel
    {
        public Guid Id { get; set; }

        // Dados pessoais
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        // Relacionamentos
        public List<EnderecoViewModel> Enderecos { get; set; } = new();
        public List<Matricula> Matriculas { get; set; } = new();
    }
}
