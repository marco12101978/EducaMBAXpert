namespace EducaMBAXpert.Core.DomainObjects.DTO
{
    public class ListaCursosPedido
    {
        public Guid CursoPedidoId { get; set; }
        public ICollection<Curso> curso { get; set; }
    }

    public class Curso
    {
        public Guid Id { get; set; }
    }
}
