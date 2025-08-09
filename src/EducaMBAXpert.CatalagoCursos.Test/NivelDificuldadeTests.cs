using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using Xunit;

namespace EducaMBAXpert.CatalagoCursos.Test
{
    public class NivelDificuldadeTests
    {
        [Fact(DisplayName = "Nível de dificuldade deve conter valores esperados")]
        [Trait("Nivel Dificuldades", "Service")]
        public void Nivel_DeveConterValores()
        {
            // Arrange
            var iniciante = NivelDificuldade.Iniciante;
            var intermediario = NivelDificuldade.Intermediario;
            var avancado = NivelDificuldade.Avancado;

            // Act
            var existeIniciante = Enum.IsDefined(typeof(NivelDificuldade), iniciante);
            var existeIntermediario = Enum.IsDefined(typeof(NivelDificuldade), intermediario);
            var existeAvancado = Enum.IsDefined(typeof(NivelDificuldade), avancado);

            // Assert
            Assert.True(existeIniciante);
            Assert.True(existeIntermediario);
            Assert.True(existeAvancado);
        }

        [Fact(DisplayName = "Conversão de string para enum deve funcionar")]
        [Trait("Nivel Dificuldades", "Service")]
        public void Nivel_ConversaoString_DeveFuncionar()
        {
            // Arrange
            var valor = "Avancado";

            // Act
            var nivel = (NivelDificuldade)Enum.Parse(typeof(NivelDificuldade), valor);

            // Assert
            Assert.Equal(NivelDificuldade.Avancado, nivel);
        }
    }
}
