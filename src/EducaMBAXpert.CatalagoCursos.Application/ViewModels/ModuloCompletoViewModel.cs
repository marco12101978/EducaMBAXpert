using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class ModuloCompletoViewModel
    {
        [Required(ErrorMessage = "O nome do módulo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "Aulas do Módulo")]
        public IEnumerable<AulaCompletoViewModel> Aulas { get; set; }
    }
}
