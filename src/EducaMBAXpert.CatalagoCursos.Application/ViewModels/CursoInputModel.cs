using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class CursoInputModel
    {
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Display(Name = "Curso Ativo")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "A categoria do curso é obrigatória.")]
        public CategoriaCurso Categoria { get; set; }

        [Required(ErrorMessage = "O nível de dificuldade é obrigatório.")]
        public NivelDificuldade Nivel { get; set; }

        [Display(Name = "Módulos do Curso")]
        [MinLength(1, ErrorMessage = "O curso deve conter pelo menos um módulo.")]
        public IEnumerable<ModuloInputModel> Modulos { get; set; } = new List<ModuloInputModel>();
    }
}
