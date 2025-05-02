using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.CatalagoCursos.Application.ViewModels
{
    public class ModuloViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Aulas do Módulo")]
        public IEnumerable<AulaViewModel> Aulas { get; set; } = new List<AulaViewModel>();
    }
}
