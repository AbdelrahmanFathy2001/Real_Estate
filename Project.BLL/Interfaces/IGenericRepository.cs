using Microsoft.EntityFrameworkCore.Storage;
using Project.BLL.Specifications;
using Project.DAL.Entities;
using System.Linq.Expressions;

namespace Project.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<IEnumerable<Image>> GetListByIdAsync(int? id);

        Task<int> GetCountAsync(ISpecification<T> spec);

        Task<int> UpdateAsync(T spec);

        Task<int> AddAsync(T spec);

        Task<int> DeleteAsync(T spec);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();

    }
}
