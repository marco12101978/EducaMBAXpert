using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Alunos.Application.ViewModels
{
    public class MatriculaInputModel
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public Guid AlunoId { get; set; }

        [Required(ErrorMessage = "O ID do curso é obrigatório.")]
        public Guid CursoId { get; set; }

    }
}
