using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain
{
    public class Curso : Entity
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public bool Ativo { get; private set; }

        public CategoriaCurso Categoria { get; private set; }
        public NivelDificuldade Nivel { get; private set; }
        public TimeSpan DuracaoTotal => CalcularDuracaoTotal();

        private readonly List<Modulo> _modulos = new();
        public IReadOnlyCollection<Modulo> Modulos => _modulos.AsReadOnly();

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        public Curso(string titulo, string descricao, decimal preco, CategoriaCurso categoria, NivelDificuldade nivel)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
            Categoria = categoria;
            Nivel = nivel;
            Ativo = true;
        }

        public void AdicionarModulo(Modulo modulo)
        {
            if (modulo == null) throw new ArgumentNullException(nameof(modulo));
            _modulos.Add(modulo);
        }

        public void Desativar() => Ativo = false;

        public void AdicionarTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                var tagNormalizada = tag.Trim().ToLower();
                if (!_tags.Contains(tagNormalizada))
                    _tags.Add(tagNormalizada);
            }
        }

        public void RemoverTag(string tag)
        {
            var tagNormalizada = tag.Trim().ToLower();
            _tags.Remove(tagNormalizada);
        }

        private TimeSpan CalcularDuracaoTotal()
        {
            return _modulos.SelectMany(m => m.Aulas).Aggregate(TimeSpan.Zero, (total, aula) => total + aula.Duracao);
        }
    }


    public enum CategoriaCurso
    {
        Programacao,
        Design,
        Marketing,
        Negocios,
        Idiomas
    }

    public enum NivelDificuldade
    {
        Iniciante,
        Intermediario,
        Avancado
    }

    public class Modulo
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }

        private readonly List<Aula> _aulas = new();
        public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();

        public Modulo(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;
        }

        public void AdicionarAula(Aula aula)
        {
            if (aula == null) throw new ArgumentNullException(nameof(aula));
            _aulas.Add(aula);
        }
    }

    public class Aula
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public TimeSpan Duracao { get; private set; }

        public Aula(string titulo, TimeSpan duracao)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Duracao = duracao;
        }
    }
}
