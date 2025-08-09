using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Alunos.Application.ViewModels
{
    public class EnderecoViewModel
    {
        // Logradouro
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

        // Localização
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        // Relacionamento
        public Guid AlunoId { get; set; }
    }
}
