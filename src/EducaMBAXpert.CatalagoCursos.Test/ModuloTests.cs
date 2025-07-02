using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class ModuloTests
    {
        [Fact(DisplayName = "Criar módulo válido")]
        [Trait("Modulo", "Service")]
        public void CriarModulo_DeveSerValido()
        {
            // Arrange
            var nome = "Módulo 1";

            // Act
            var modulo = new Modulo(nome);

            // Assert
            Assert.Equal(nome, modulo.Nome);
            Assert.Empty(modulo.Aulas);
        }

        [Theory(DisplayName = "Nome inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Modulo", "Service")]
        public void CriarModulo_NomeInvalido_DeveLancarExcecao(string nome)
        {
            // Arrange & Act
            var ex = Assert.Throws<DomainException>(() => new Modulo(nome));

            // Assert
            Assert.Equal("O campo Nome não pode ser vazio", ex.Message);
        }

        [Fact(DisplayName = "Adicionar aula válida")]
        [Trait("Modulo", "Service")]
        public void AdicionarAula_Valida_DeveAdicionar()
        {
            // Arrange
            var modulo = new Modulo("Módulo de Teste");
            var aula = new Aula("Aula 1", "https://url.com/aula1", TimeSpan.FromMinutes(20));

            // Act
            modulo.AdicionarAula(aula);

            // Assert
            Assert.Single(modulo.Aulas);
            Assert.Contains(aula, modulo.Aulas);
        }

        [Fact(DisplayName = "Adicionar aula nula deve lançar exceção")]
        [Trait("Modulo", "Service")]
        public void AdicionarAula_Nula_DeveLancarExcecao()
        {
            // Arrange
            var modulo = new Modulo("Módulo");

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => modulo.AdicionarAula(null));

            // Assert
            Assert.Equal("aula", ex.ParamName);
        }
    }
}
