namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class MatriculaViewModel
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid CursoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public bool Ativo { get; set; }

    }
}
