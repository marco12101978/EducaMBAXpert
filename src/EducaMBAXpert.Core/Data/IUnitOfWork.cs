namespace EducaMBAXpert.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
