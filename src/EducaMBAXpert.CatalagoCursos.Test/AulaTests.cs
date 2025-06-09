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
            // Arrange & Act
            var aula = new Aula("Introdução ao C#", "https://video.com/introducao", TimeSpan.FromMinutes(10));

            // Assert
            Assert.Equal("Introdução ao C#", aula.Titulo);
            Assert.Equal("https://video.com/introducao", aula.Url);
            Assert.Equal(TimeSpan.FromMinutes(10), aula.Duracao);
        }

        [Theory(DisplayName = "Título inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Aula", "Service")]
        public void CriarAula_TituloInvalido_DeveLancarExcecao(string tituloInvalido)
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Aula(tituloInvalido, "https://video.com/aula", TimeSpan.FromMinutes(5)));

            Assert.Equal("O campo Titulo não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "URL inválida")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Aula", "Service")]
        public void CriarAula_UrlInvalida_DeveLancarExcecao(string urlInvalida)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Aula("Aula válida", urlInvalida, TimeSpan.FromMinutes(5)));

            Assert.Equal("O campo URL não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Duração inválida")]
        [InlineData(0)]
        [InlineData(-1)]
        [Trait("Aula", "Service")]
        public void CriarAula_DuracaoInvalida_DeveLancarExcecao(int minutos)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Aula("Aula válida", "https://video.com/aula", TimeSpan.FromMinutes(minutos)));

            Assert.Equal("O Campo Duracao nao pode ser menor ou igual a 0 minutos", ex.Message);
        }
    }
}
