using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class CursoViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
        public CategoriaCurso Categoria { get; set; }
        public NivelDificuldade Nivel { get; set; }
        public TimeSpan DuracaoTotal { get; set; }
        public IEnumerable<ModuloViewModel> Modulos { get; set; } = new List<ModuloViewModel>();

    }
}
