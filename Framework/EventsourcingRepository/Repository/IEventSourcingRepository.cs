using SharedKernel;
using System;
using System.Threading.Tasks;

namespace EventSourcingRepository.Repository
{
    public interface IEventSourcingRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        Task<TAggregate> FindByIdAsync(Guid id);
        Task SaveAsync(TAggregate aggregate);
    }
}
