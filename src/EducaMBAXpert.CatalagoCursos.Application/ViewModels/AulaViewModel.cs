using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class AulaViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título da aula é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A URL da aula é obrigatória.")]
        [Url(ErrorMessage = "A URL da aula não é válida.")]
        public string Url { get; set; }

        [Display(Name = "Duração da Aula")]
        [Required(ErrorMessage = "A duração da aula é obrigatória.")]
        public TimeSpan Duracao { get; set; }
    }
}
