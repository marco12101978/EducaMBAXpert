using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Entities
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public string Url { get; private set; }
        public TimeSpan Duracao { get; private set; }

        public Aula(string titulo, string url, TimeSpan duracao)
        {
            Validacoes.ValidarSeVazio(titulo, "O campo Titulo não pode ser vazio");
            Validacoes.ValidarSeVazio(url, "O campo URL não pode ser vazio");
            Validacoes.ValidarSeMenorOuIgualQue(duracao, TimeSpan.Zero, "O Campo Duracao nao pode ser menor ou igual a 0 minutos");

            Titulo = titulo;
            Duracao = duracao;
        }
    }
}
