using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class EntityTests
    {
        private class EntityFake : Entity { }

        [Fact(DisplayName = "Entidade deve ter ID gerado")]
        [Trait("Entity", "Core")]
        public void Entidade_DeveGerarId()
        {
            // Arrange & Act
            var entidade = new EntityFake();

            // Assert
            Assert.NotEqual(Guid.Empty, entidade.Id);
        }

        [Fact(DisplayName = "Entidades com mesmo ID devem ser iguais")]
        [Trait("Entity", "Core")]
        public void Entidades_ComMesmoId_DevemSerIguais()
        {
            // Arrange
            var id = Guid.NewGuid();
            var a = new EntityFake { Id = id };
            var b = new EntityFake { Id = id };

            // Act
            var saoIguais = a.Equals(b);
            var operadorIgual = a == b;
            var operadorDiferente = a != b;

            // Assert
            Assert.True(saoIguais);
            Assert.True(operadorIgual);
            Assert.False(operadorDiferente);
        }

        [Fact(DisplayName = "Entidades com IDs diferentes devem ser diferentes")]
        [Trait("Entity", "Core")]
        public void Entidades_ComIdsDiferentes_DevemSerDiferentes()
        {
            // Arrange
            var a = new EntityFake();
            var b = new EntityFake();

            // Act
            var saoDiferentes = !a.Equals(b);
            var operadorIgual = a == b;
            var operadorDiferente = a != b;

            // Assert
            Assert.True(saoDiferentes);
            Assert.False(operadorIgual);
            Assert.True(operadorDiferente);
        }
    }
}
