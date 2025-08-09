using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Alunos.Domain.Entities
{
    public class Endereco : Entity
    {
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public Guid AlunoId { get; private set; }
        public Aluno Aluno { get; private set; }

        public Endereco(string rua, string numero, string complemento, string bairro, string cidade, string estado, string cep, Guid alunoId)
        {
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            AlunoId = alunoId;
            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Rua, "Rua inválida");
            Validacoes.ValidarSeVazio(Cidade, "Cidade inválida");
            Validacoes.ValidarSeVazio(Bairro, "Bairro inválido");
            Validacoes.ValidarSeVazio(Estado, "Estado inválido");
            Validacoes.ValidarSeVazio(Cep, "CEP inválido");
        }
    }
}
