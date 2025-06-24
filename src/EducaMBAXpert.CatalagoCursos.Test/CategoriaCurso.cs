using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class CategoriaCursoTests
    {
        [Fact(DisplayName = "Categoria deve conter valores esperados")]
        [Trait("Categoria", "Service")]
        public void Categoria_DeveConterValores()
        {
            // Arrange & Act & Assert
            Assert.True(Enum.IsDefined(typeof(CategoriaCurso), CategoriaCurso.Programacao));
            Assert.True(Enum.IsDefined(typeof(CategoriaCurso), CategoriaCurso.Design));
            Assert.True(Enum.IsDefined(typeof(CategoriaCurso), CategoriaCurso.Marketing));
            Assert.True(Enum.IsDefined(typeof(CategoriaCurso), CategoriaCurso.Negocios));
            Assert.True(Enum.IsDefined(typeof(CategoriaCurso), CategoriaCurso.Idiomas));
        }

        [Fact(DisplayName = "Conversão de string para enum deve funcionar")]
        [Trait("Categoria", "Service")]
        public void Categoria_ConversaoString_DeveFuncionar()
        {
            // Arrange
            var valor = "Marketing";

            // Act
            var categoria = (CategoriaCurso)Enum.Parse(typeof(CategoriaCurso), valor);

            // Assert
            Assert.Equal(CategoriaCurso.Marketing, categoria);
        }
    }
}
