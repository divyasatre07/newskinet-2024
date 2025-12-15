using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.interfaces;

namespace Core.Interfaces
{

    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetEntityWithspec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
		Task<TResult?> GetEntityWithspec<TResult>(ISpecification<T,TResult> spec);
		Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T,TResult> spec);


		void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<bool> SaveAllAsync();

          bool Exists(int id);
        Task <int> CountAsync(ISpecification<T> spec);
        
    }
}
