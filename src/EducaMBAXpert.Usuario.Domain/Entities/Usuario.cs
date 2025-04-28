using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Usuarios.Domain.Entities
{
    public class Usuario : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
    
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool Ativo { get; private set; }

        //private readonly List<Endereco> _enderecos = new();
        //public IReadOnlyCollection<Endereco> Enderecos => _enderecos.AsReadOnly();
        public List<Endereco> Enderecos { get; private set; } = new List<Endereco>();

        public Usuario(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Ativo = true;

            Validar();
        }

        public void Inativar() => Ativo = false;
        public void Ativar() => Ativo = true;

        public void AlterarNome(string nome)
        {
            Validacoes.ValidarSeVazio(nome, "Nome não pode ser vazio");

            Nome = nome;
        }

        public void AlterarEmail(string email)
        {
            Validacoes.ValidarEmail(email, "E-mail inválido.");

            Email = email;
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            Validacoes.ValidarSeNulo(endereco, "Endereço inválido.");

            Enderecos.Add(endereco);
        }

        public void RemoverEndereco(Endereco endereco)
        {
            Validacoes.ValidarSeNulo(endereco, "Endereço inválido.");

            Enderecos.Remove(endereco);
        }

        public void Validar()
        {
            Validacoes.ValidarGuid(Id, "ID do usuario inválido.");
            Validacoes.ValidarSeVazio(Nome, "Nome não pode ser vazio");
            Validacoes.ValidarSeVazio(Email, "Email não pode ser vazio");
        }

    }
}
