using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
