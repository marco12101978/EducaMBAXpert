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
            // Arrange
            var titulo = "Curso C#";
            var descricao = "Aprenda C#";
            var valor = 199.90m;
            var categoria = CategoriaCurso.Programacao;
            var nivel = NivelDificuldade.Iniciante;

            // Act
            var curso = new Curso(titulo, descricao, valor, categoria, nivel);

            // Assert
            Assert.Equal(titulo, curso.Titulo);
            Assert.True(curso.Valor > 0);
            Assert.True(curso.Ativo);
        }

        [Theory(DisplayName = "Título inválido")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Curso", "Service")]
        public void CriarCurso_TituloInvalido_DeveLancarExcecao(string titulo)
        {
            // Arrange
            var descricao = "desc";
            var valor = 100m;
            var categoria = CategoriaCurso.Programacao;
            var nivel = NivelDificuldade.Iniciante;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Curso(titulo, descricao, valor, categoria, nivel));

            // Assert
            Assert.Equal("O campo Titulo não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Descrição inválida")]
        [InlineData("")]
        [InlineData(null)]
        [Trait("Curso", "Service")]
        public void CriarCurso_DescricaoInvalida_DeveLancarExcecao(string descricao)
        {
            // Arrange
            var titulo = "Curso";
            var valor = 100m;
            var categoria = CategoriaCurso.Programacao;
            var nivel = NivelDificuldade.Iniciante;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Curso(titulo, descricao, valor, categoria, nivel));

            // Assert
            Assert.Equal("O campo Descrição não pode ser vazio", ex.Message);
        }

        [Theory(DisplayName = "Valor inválido")]
        [InlineData(0)]
        [InlineData(-10)]
        [Trait("Curso", "Service")]
        public void CriarCurso_ValorInvalido_DeveLancarExcecao(decimal valor)
        {
            // Arrange
            var titulo = "Curso";
            var descricao = "Descricao";
            var categoria = CategoriaCurso.Programacao;
            var nivel = NivelDificuldade.Iniciante;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Curso(titulo, descricao, valor, categoria, nivel));

            // Assert
            Assert.Equal("O valor do curso deve ser maior que zero", ex.Message);
        }

        [Fact(DisplayName = "Categoria nula")]
        [Trait("Curso", "Service")]
        public void CriarCurso_CategoriaNula_DeveLancarExcecao()
        {
            // Arrange
            var titulo = "Curso";
            var descricao = "Descricao";
            var valor = 100m;
            CategoriaCurso? categoria = null;
            var nivel = NivelDificuldade.Iniciante;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Curso(titulo, descricao, valor, categoria, nivel));

            // Assert
            Assert.Equal("Categoria é obrigatória", ex.Message);
        }

        [Fact(DisplayName = "Nível nulo")]
        [Trait("Curso", "Service")]
        public void CriarCurso_NivelNulo_DeveLancarExcecao()
        {
            // Arrange
            var titulo = "Curso";
            var descricao = "Descricao";
            var valor = 100m;
            var categoria = CategoriaCurso.Programacao;
            NivelDificuldade? nivel = null;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                     new Curso(titulo, descricao, valor, categoria, nivel));

            // Assert
            Assert.Equal("Nível de dificuldade é obrigatório", ex.Message);
        }
    }
}
