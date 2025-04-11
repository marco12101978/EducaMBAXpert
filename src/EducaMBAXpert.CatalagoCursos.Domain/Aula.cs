using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public TimeSpan Duracao { get; private set; }

        public Aula(string titulo, TimeSpan duracao)
        {
            Validacoes.ValidarSeVazio(titulo, "O campo Titulo não pode ser vazio");
            Validacoes.ValidarSeMenorOuIgualQue(duracao, TimeSpan.Zero, "O Campo Duracao nao pode ser menor ou igual a 0 minutos");

            Titulo = titulo;
            Duracao = duracao;
        }
    }
}
