using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Entities;
using System.Collections;

namespace Project.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _dbContext;
        //private Dictionary<string, GenericRepository<BaseEntity>> _repository;
        private Hashtable _repository;  // obj, obj 

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new Hashtable();

        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repository.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repository.Add(key, repository);
            }

            return _repository[key] as IGenericRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()
           => await _dbContext.SaveChangesAsync();


        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
