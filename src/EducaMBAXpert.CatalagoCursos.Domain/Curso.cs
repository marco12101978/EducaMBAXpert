﻿using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain
{
    public class Curso : Entity, IAggregateRoot
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

            Validar();
        }

        public void AdicionarModulo(Modulo modulo)
        {
            if (modulo == null) throw new ArgumentNullException(nameof(modulo));
            _modulos.Add(modulo);
        }

        public void Desativar() => Ativo = false;

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
            Validacoes.ValidarSeMenorQue(Preco, 0, "O campo Preco não pode ser menor que 0");
        }
    }
}
