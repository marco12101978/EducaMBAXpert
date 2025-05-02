using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class ModuloInputModel
    {
        [Required(ErrorMessage = "O nome do módulo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Aulas do Módulo")]
        [MinLength(1, ErrorMessage = "O módulo deve conter pelo menos uma aula.")]
        public IEnumerable<AulaInputModel> Aulas { get; set; } = new List<AulaInputModel>();
    }
}
