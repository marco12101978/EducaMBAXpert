using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class AulaViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo URL é obrigatório.")]
        [Url(ErrorMessage = "A URL fornecida não é válida.")]
        public string Url { get; set; } = string.Empty;

        [Display(Name = "Duração da Aula")]
        [Required(ErrorMessage = "O campo Duração é obrigatório.")]
        public TimeSpan Duracao { get; set; }
    }
}
