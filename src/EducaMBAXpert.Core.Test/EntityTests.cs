using EducaMBAXpert.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var entidade = new EntityFake();
            Assert.NotEqual(Guid.Empty, entidade.Id);
        }

        [Fact(DisplayName = "Entidades com mesmo ID devem ser iguais")]
        [Trait("Entity", "Core")]
        public void Entidades_ComMesmoId_DevemSerIguais()
        {
            var id = Guid.NewGuid();
            var a = new EntityFake { Id = id };
            var b = new EntityFake { Id = id };

            Assert.Equal(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
        }

        [Fact(DisplayName = "Entidades com IDs diferentes devem ser diferentes")]
        [Trait("Entity", "Core")]
        public void Entidades_ComIdsDiferentes_DevemSerDiferentes()
        {
            var a = new EntityFake();
            var b = new EntityFake();

            Assert.NotEqual(a, b);
            Assert.False(a == b);
            Assert.True(a != b);
        }
    }
}
