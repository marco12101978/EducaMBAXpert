using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    public class Modulo : Entity
    {
        public string Nome { get; private set; }

        private readonly List<Aula> _aulas = new();
        public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();

        public Modulo(string nome)
        {
            Validacoes.ValidarSeVazio(nome, "O campo Nome não pode ser vazio");

            Nome = nome;
        }

        public void AdicionarAula(Aula aula)
        {
            if (aula == null) throw new ArgumentNullException(nameof(aula));
            _aulas.Add(aula);
        }

    }
}
