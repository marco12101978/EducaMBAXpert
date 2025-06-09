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
            var tag = new Tag("backend");
            Assert.Equal("backend", tag.Valor);
        }

        [Fact(DisplayName = "Criar tag com valor nulo deve lançar exceção")]
        [Trait("Tags", "Service")]
        public void CriarTag_ValorNulo_DeveLancarExcecao()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Tag(null));
            Assert.Equal("valor", ex.ParamName);
        }

        [Fact(DisplayName = "Tags com mesmo valor devem ser iguais")]
        [Trait("Tags", "Service")]
        public void Tags_Iguais_DeveRetornarTrue()
        {
            var tag1 = new Tag("frontend");
            var tag2 = new Tag("frontend");

            Assert.Equal(tag1, tag2);
        }

        [Fact(DisplayName = "Tags com valores diferentes devem ser diferentes")]
        [Trait("Tags", "Service")]
        public void Tags_Diferentes_DeveRetornarFalse()
        {
            var tag1 = new Tag("frontend");
            var tag2 = new Tag("backend");

            Assert.NotEqual(tag1, tag2);
        }
    }
}
