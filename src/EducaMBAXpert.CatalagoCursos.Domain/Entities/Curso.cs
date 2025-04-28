using EducaMBAXpert.CatalagoCursos.Domain.Events;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    public class Curso : Entity, IAggregateRoot
    {
        public Curso(string titulo, string descricao, CategoriaCurso categoria, NivelDificuldade nivel)
        {

            Titulo = titulo;
            Descricao = descricao;
            Categoria = categoria;
            Nivel = nivel;
            Ativo = true;

            Validar();
        }


        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }

        public CategoriaCurso Categoria { get; private set; }
        public NivelDificuldade Nivel { get; private set; }
        public TimeSpan DuracaoTotal => CalcularDuracaoTotal();

        private readonly List<Modulo> _modulos = new();
        public IReadOnlyCollection<Modulo> Modulos => _modulos.AsReadOnly();

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();


        public void AdicionarModulo(Modulo modulo)
        {
            if (modulo == null) throw new ArgumentNullException(nameof(modulo));
            _modulos.Add(modulo);
        }

        public void Inativar() => Ativo = false;
        public void Ativar() => Ativo = true;

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

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Titulo não pode ser vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descrição não pode ser vazio");
        }
    }
}
