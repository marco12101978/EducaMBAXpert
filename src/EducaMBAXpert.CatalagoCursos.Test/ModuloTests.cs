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
            var modulo = new Modulo("Módulo 1");
            Assert.Equal("Módulo 1", modulo.Nome);
            Assert.Empty(modulo.Aulas);
        }

        [Theory(DisplayName = "Nome inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Modulo", "Service")]
        public void CriarModulo_NomeInvalido_DeveLancarExcecao(string nome)
        {
            var ex = Assert.Throws<DomainException>(() => new Modulo(nome));
            Assert.Equal("O campo Nome não pode ser vazio", ex.Message);
        }

        [Fact(DisplayName = "Adicionar aula válida")]
        [Trait("Modulo", "Service")]
        public void AdicionarAula_Valida_DeveAdicionar()
        {
            var modulo = new Modulo("Módulo de Teste");
            var aula = new Aula("Aula 1", "https://url.com/aula1", TimeSpan.FromMinutes(20));

            modulo.AdicionarAula(aula);

            Assert.Single(modulo.Aulas);
            Assert.Contains(aula, modulo.Aulas);
        }

        [Fact(DisplayName = "Adicionar aula nula deve lançar exceção")]
        [Trait("Modulo", "Service")]
        public void AdicionarAula_Nula_DeveLancarExcecao()
        {
            var modulo = new Modulo("Módulo");

            var ex = Assert.Throws<ArgumentNullException>(() => modulo.AdicionarAula(null));

            Assert.Equal("aula", ex.ParamName);
        }
    }
}
