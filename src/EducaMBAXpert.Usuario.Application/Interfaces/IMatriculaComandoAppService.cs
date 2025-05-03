namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface IMatriculaComandoAppService
    {
        Task ConcluirAula(Guid matriculaId, Guid aulaId);
    }
}
