using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class CursoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Curso Ativo")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "A categoria do curso é obrigatória.")]
        public CategoriaCurso Categoria { get; set; }

        [Required(ErrorMessage = "O nível de dificuldade é obrigatório.")]
        public NivelDificuldade Nivel { get; set; }

        [Display(Name = "Duração Total")]
        public TimeSpan DuracaoTotal { get; set; } 

        [Display(Name = "Módulos do Curso")]
        public IEnumerable<ModuloViewModel> Modulos { get; set; }

        public IEnumerable<string> Tags { get; set; } 
    }
}
