using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using System;

namespace EducaMBAXpert.CatalagoCursos.Application
{
    internal class Class1
    {
        private void TTTT()
        {
            var curso = new Curso("ASP.NET Core Web API",
                                    "Aprenda a criar APIs REST com ASP.NET",
                                    CategoriaCurso.Programacao,
                                    NivelDificuldade.Intermediario
                                );

            var modulo1 = new Modulo("Fundamentos");
            modulo1.AdicionarAula(new Aula("Introdução às APIs", "https://www.youtube.com/watch?v=H9dHQlWq9do&list=PLIICLSbtN_uFPLnXg7VPq30pMymwCd8hX", TimeSpan.FromMinutes(20)));
            modulo1.AdicionarAula(new Aula("Controllers e Rotas", "https://www.youtube.com/watch?v=0_V-xHiRbgY&list=PLIICLSbtN_uFPLnXg7VPq30pMymwCd8hX&index=2", TimeSpan.FromMinutes(30)));

            curso.AdicionarModulo(modulo1);

            curso.AdicionarTag("csharp");
            curso.AdicionarTag("backend");
            curso.AdicionarTag("dotnet");

            Console.WriteLine($"Duração total do curso: {curso.DuracaoTotal.TotalMinutes} minutos");
        }


        //public static void SeedCursos(AppDbContext context)
        //{
        //    if (!context.Cursos.Any())
        //    {
        //        var curso = new Curso
        //        {
        //            Titulo = "Curso de C# com EF Core",
        //            Descricao = "Aprenda C# e como usar o Entity Framework Core",
        //            Categoria = "Programação",
        //            Nivel = "Intermediário",
        //            DuracaoTotal = TimeSpan.FromHours(5)
        //        };

        //        var modulo1 = new Modulo
        //        {
        //            Nome = "Introdução ao C#",
        //            Curso = curso
        //        };

        //        modulo1.Aulas.Add(new Aula { Titulo = "O que é C#", Duracao = TimeSpan.FromMinutes(30) });
        //        modulo1.Aulas.Add(new Aula { Titulo = "Primeiro Projeto", Duracao = TimeSpan.FromMinutes(45) });

        //        var modulo2 = new Modulo
        //        {
        //            Nome = "Trabalhando com EF Core",
        //            Curso = curso
        //        };

        //        modulo2.Aulas.Add(new Aula { Titulo = "DbContext e Migrations", Duracao = TimeSpan.FromMinutes(40) });
        //        modulo2.Aulas.Add(new Aula { Titulo = "Relacionamentos", Duracao = TimeSpan.FromMinutes(50) });

        //        curso.Modulos.Add(modulo1);
        //        curso.Modulos.Add(modulo2);

        //        context.Cursos.Add(curso);
        //        context.SaveChanges();
        //    }
        //}

    }
}
