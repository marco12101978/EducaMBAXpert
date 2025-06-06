﻿using EducaMBAXpert.CatalagoCursos.Domain.Entities.EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    public class Curso : Entity, IAggregateRoot
    {
        public Curso(string titulo, string descricao,decimal valor, CategoriaCurso? categoria, NivelDificuldade? nivel)
        {
            Titulo = titulo;
            Descricao = descricao;
            Categoria = categoria;
            Nivel = nivel;
            Ativo = true;
            Valor = valor;

            Validar();
        }


        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }

        public bool Ativo { get; private set; }

        public CategoriaCurso? Categoria { get; private set; }
        public NivelDificuldade? Nivel { get; private set; }
        public TimeSpan DuracaoTotal => CalcularDuracaoTotal();

        private readonly List<Modulo> _modulos = new();
        public IReadOnlyCollection<Modulo> Modulos => _modulos.AsReadOnly();

        public void AdicionarModulo(Modulo modulo)
        {
            if (modulo == null) throw new ArgumentNullException(nameof(modulo));
            _modulos.Add(modulo);
        }

        public void Inativar() => Ativo = false;
        public void Ativar() => Ativo = true;

        public void AjustarPreco(decimal valor)
        {
            Valor = valor;
        }

        private TimeSpan CalcularDuracaoTotal()
        {
            return _modulos.SelectMany(m => m.Aulas).Aggregate(TimeSpan.Zero, (total, aula) => total + aula.Duracao);
        }

        public void Validar()
        {
            Validacoes.ValidarSeNulo(Categoria,"Categoria é obrigatória");
            Validacoes.ValidarSeNulo(Nivel, "Nível de dificuldade é obrigatório");

            Validacoes.ValidarSeVazio(Titulo, "O campo Titulo não pode ser vazio");

            Validacoes.ValidarSeNulo(Descricao, "O campo Descrição não pode ser vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descrição não pode ser vazio");

            Validacoes.ValidarSeMenorQue(Valor,0.01m, "O valor do curso deve ser maior que zero");
        }
    }
}
