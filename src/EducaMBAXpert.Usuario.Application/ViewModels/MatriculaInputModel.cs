using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class MatriculaInputModel
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O ID do curso é obrigatório.")]
        public Guid CursoId { get; set; }

    }
}
