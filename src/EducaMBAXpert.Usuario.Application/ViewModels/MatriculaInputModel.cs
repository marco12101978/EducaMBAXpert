using System.ComponentModel.DataAnnotations;

namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class MatriculaInputModel
    {
        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo Curso é obrigatório.")]
        public Guid CursoId { get; set; }

    }
}
