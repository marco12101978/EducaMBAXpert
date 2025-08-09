using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class AulaTests
    {
        [Fact(DisplayName = "Criar aula válida")]
        [Trait("Aula", "Service")]
        public void CriarAula_DeveSerValida()
        {
            // Arrange
            var titulo = "Introdução ao C#";
            var url = "https://video.com/introducao";
            var duracao = TimeSpan.FromMinutes(10);

            // Act
            var aula = new Aula(titulo, url, duracao);

            // Assert
            Assert.Equal(titulo, aula.Titulo);
            Assert.Equal(url, aula.Url);
            Assert.Equal(duracao, aula.Duracao);
        }

        [Theory(DisplayName = "Título inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Aula", "Service")]
        public void CriarAula_TituloInvalido_DeveLancarExcecao(string tituloInvalido)
        {
            // Arrange
            var url = "https://video.com/aula";
            var duracao = TimeSpan.FromMinutes(5);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Aula(tituloInvalido, url, duracao));

            // Assert
            Assert.Equal("O campo Titulo não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "URL inválida")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Aula", "Service")]
        public void CriarAula_UrlInvalida_DeveLancarExcecao(string urlInvalida)
        {
            // Arrange
            var titulo = "Aula válida";
            var duracao = TimeSpan.FromMinutes(5);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Aula(titulo, urlInvalida, duracao));

            // Assert
            Assert.Equal("O campo URL não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Duração inválida")]
        [InlineData(0)]
        [InlineData(-1)]
        [Trait("Aula", "Service")]
        public void CriarAula_DuracaoInvalida_DeveLancarExcecao(int minutos)
        {
            // Arrange
            var titulo = "Aula válida";
            var url = "https://video.com/aula";
            var duracao = TimeSpan.FromMinutes(minutos);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Aula(titulo, url, duracao));

            // Assert
            Assert.Equal("O Campo Duracao nao pode ser menor ou igual a 0 minutos", ex.Message);
        }
    }
}
