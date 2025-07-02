using EducaMBAXpert.CatalagoCursos.Domain.Entities.EducaMBAXpert.CatalagoCursos.Domain.Entities;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class TagTests
    {
        [Fact(DisplayName = "Criar tag válida")]
        [Trait("Tags", "Service")]
        public void CriarTag_Valida_DeveSerValida()
        {
            // Arrange
            var valor = "backend";

            // Act
            var tag = new Tag(valor);

            // Assert
            Assert.Equal(valor, tag.Valor);
        }

        [Fact(DisplayName = "Criar tag com valor nulo deve lançar exceção")]
        [Trait("Tags", "Service")]
        public void CriarTag_ValorNulo_DeveLancarExcecao()
        {
            // Arrange
            string valorNulo = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new Tag(valorNulo));

            // Assert
            Assert.Equal("valor", ex.ParamName);
        }

        [Fact(DisplayName = "Tags com mesmo valor devem ser iguais")]
        [Trait("Tags", "Service")]
        public void Tags_Iguais_DeveRetornarTrue()
        {
            // Arrange
            var valor = "frontend";

            // Act
            var tag1 = new Tag(valor);
            var tag2 = new Tag(valor);

            // Assert
            Assert.Equal(tag1, tag2);
        }

        [Fact(DisplayName = "Tags com valores diferentes devem ser diferentes")]
        [Trait("Tags", "Service")]
        public void Tags_Diferentes_DeveRetornarFalse()
        {
            // Arrange
            var valor1 = "frontend";
            var valor2 = "backend";

            // Act
            var tag1 = new Tag(valor1);
            var tag2 = new Tag(valor2);

            // Assert
            Assert.NotEqual(tag1, tag2);
        }
    }
}
