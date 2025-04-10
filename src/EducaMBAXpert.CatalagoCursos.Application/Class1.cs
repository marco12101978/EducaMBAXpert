using EducaMBAXpert.CatalagoCursos.Domain;

namespace EducaMBAXpert.CatalagoCursos.Application
{
    internal class Class1
    {

        private void TTTT()
        {
            var curso = new Curso("ASP.NET Core Web API",
                                    "Aprenda a criar APIs REST com ASP.NET",
                                    249.90m,
                                    CategoriaCurso.Programacao,
                                    NivelDificuldade.Intermediario
                                );

            var modulo1 = new Modulo("Fundamentos");
            modulo1.AdicionarAula(new Aula("Introdução às APIs", TimeSpan.FromMinutes(20)));
            modulo1.AdicionarAula(new Aula("Controllers e Rotas", TimeSpan.FromMinutes(30)));

            curso.AdicionarModulo(modulo1);

            curso.AdicionarTag("csharp");
            curso.AdicionarTag("backend");
            curso.AdicionarTag("dotnet");

            Console.WriteLine($"Duração total do curso: {curso.DuracaoTotal.TotalMinutes} minutos");
        }

    }
}
