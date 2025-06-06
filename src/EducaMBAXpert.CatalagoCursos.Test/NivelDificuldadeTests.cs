using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class NivelDificuldadeTests
    {
        [Fact(DisplayName = "Nível de dificuldade deve conter valores esperados")]
        public void Nivel_DeveConterValores()
        {
            Assert.True(Enum.IsDefined(typeof(NivelDificuldade), NivelDificuldade.Iniciante));
            Assert.True(Enum.IsDefined(typeof(NivelDificuldade), NivelDificuldade.Intermediario));
            Assert.True(Enum.IsDefined(typeof(NivelDificuldade), NivelDificuldade.Avancado));
        }

        [Fact(DisplayName = "Conversão de string para enum deve funcionar")]
        public void Nivel_ConversaoString_DeveFuncionar()
        {
            var nivel = (NivelDificuldade)Enum.Parse(typeof(NivelDificuldade), "Avancado");
            Assert.Equal(NivelDificuldade.Avancado, nivel);
        }
    }
}
