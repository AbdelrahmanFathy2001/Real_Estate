using Project.DAL.Entities;

namespace Project.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        //public IGenericRepository<Product> ProductsRepo { get; set; }
        //public IGenericRepository<Brand> BrandsRepo { get; set; }
        //public IGenericRepository<Category> CategoriesRepo { get; set; }
        //public IGenericRepository<DeliveryMethod> DeliveryMethodsRepo { get; set; }
        //public IGenericRepository<OrderItem> OrderItemsRepo { get; set; }
        //public IGenericRepository<Order> OrdersRepo { get; set; }

        Task<int> CompleteAsync();
    }
}
