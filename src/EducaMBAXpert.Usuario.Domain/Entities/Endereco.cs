using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Usuarios.Domain.Entities
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
        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public Endereco(string rua, string numero, string complemento, string bairro, string cidade, string estado, string cep, Guid usuarioId)
        {
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            UsuarioId = usuarioId;
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
