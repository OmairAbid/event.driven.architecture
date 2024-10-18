using System;
using System.Threading.Tasks;

namespace QueryRepository.Repository
{
    public interface IProjectionRepository<T> : IQueryRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid Id);
    }
}
