namespace EducaMBAXpert.Usuarios.Application.ViewModels
{
    public class MatriculaViewModel
    {
        public Guid Id { get; set; }

        // Relacionamento
        public Guid UsuarioId { get; set; }
        public Guid CursoId { get; set; }

        // Dados da matrícula
        public DateTime DataMatricula { get; set; }
        public bool Ativo { get; set; }

    }
}
