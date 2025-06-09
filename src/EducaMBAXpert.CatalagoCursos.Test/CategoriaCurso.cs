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
            var categoria = (CategoriaCurso)Enum.Parse(typeof(CategoriaCurso), "Marketing");
            Assert.Equal(CategoriaCurso.Marketing, categoria);
        }
    }
}
