using EducaMBAXpert.Core.DomainObjects;
using System;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public string Url { get; private set; }
        public TimeSpan Duracao { get; private set; }

        public Aula(string titulo, string url, TimeSpan duracao)
        {
            Titulo = titulo;
            Duracao = duracao;
            Url = url;

            Validar();
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Titulo, "O campo Titulo não pode ser vazio");
            Validacoes.ValidarSeVazio(Url, "O campo URL não pode ser vazio");
            Validacoes.ValidarSeMenorOuIgualQue(Duracao, TimeSpan.Zero, "O Campo Duracao nao pode ser menor ou igual a 0 minutos");
        }
    }
}
