using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class CursoTests
    {

        [Fact(DisplayName = "Criar curso válido")]
        [Trait("Curso", "Service")]
        public void CriarCurso_DeveSerValido()
        {
            var curso = new Curso("Curso C#", "Aprenda C#", 199.90m, CategoriaCurso.Programacao, NivelDificuldade.Iniciante);

            Assert.Equal("Curso C#", curso.Titulo);
            Assert.True(curso.Valor > 0);
            Assert.True(curso.Ativo);
        }

        [Theory(DisplayName = "Título inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Curso", "Service")]
        public void CriarCurso_TituloInvalido_DeveLancarExcecao(string titulo)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Curso(titulo, "desc", 100, CategoriaCurso.Programacao, NivelDificuldade.Iniciante));

            Assert.Equal("O campo Titulo não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Descrição inválida")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Curso", "Service")]
        public void CriarCurso_DescricaoInvalida_DeveLancarExcecao(string descricao)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Curso("Curso", descricao, 100, CategoriaCurso.Programacao, NivelDificuldade.Iniciante));

            Assert.Equal("O campo Descrição não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Valor inválido")]
        [InlineData(0)]
        [InlineData(-10)]
        [Trait("Curso", "Service")]
        public void CriarCurso_ValorInvalido_DeveLancarExcecao(decimal valor)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Curso("Curso", "Descricao", valor, CategoriaCurso.Programacao, NivelDificuldade.Iniciante));

            Assert.Equal("O valor do curso deve ser maior que zero", ex.Message);
        }

        [Fact(DisplayName = "Categoria nula")]
        [Trait("Curso", "Service")]
        public void CriarCurso_CategoriaNula_DeveLancarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Curso("Curso", "Descricao", 100, null, NivelDificuldade.Iniciante));

            Assert.Equal("Categoria é obrigatória", ex.Message);
        }

        [Fact(DisplayName = "Nível nulo")]
        [Trait("Curso", "Service")]
        public void CriarCurso_NivelNulo_DeveLancarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Curso("Curso", "Descricao", 100, CategoriaCurso.Programacao, null));

            Assert.Equal("Nível de dificuldade é obrigatório", ex.Message);
        }
    }
}
