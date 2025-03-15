using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.BLL.Interfaces;
using Project.BLL.Specifications;
using Project.DAL.Data;
using Project.DAL.Entities;
using System.Linq.Expressions;

namespace Project.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
            => await context.Set<T>().FindAsync(id);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {

            try
            {
                // Log or output spec values for debugging
                Console.WriteLine($"Specification values: {spec}");

                // Execute the query and return results
                return await ApplySpecifications(spec).ToListAsync();
            }
            catch (ArgumentException ex)
            {
                // Log specific argument errors
                Console.WriteLine($"ArgumentException encountered: {ex.Message}");
                throw;
            }


        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Image>> GetListByIdAsync(int? id)
            => await context.Set<Image>().Where(x => x.RealEstateAdId == id).ToListAsync();


        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).CountAsync();


        public async Task<int> AddAsync(T spec)
        {
            context.Set<T>().Add(spec);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T spec)
        {
            context.Set<T>().Remove(spec);
            return await context.SaveChangesAsync();
        }




        ///////////////////////////////////////////////////////////////
        ///
        #region Actions


        public IQueryable<T> GetTableNoTracking()
        {
            return context.Set<T>().AsNoTracking().AsQueryable();
        }


        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public void Commit()
        {
            context.Database.CommitTransaction();

        }

        public void RollBack()
        {
            context.Database.RollbackTransaction();
        }

        public IQueryable<T> GetTableAsTracking()
        {
            return context.Set<T>().AsQueryable();

        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollBackAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }
        #endregion














    }
}
