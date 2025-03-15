
using System.Linq.Expressions;


namespace Project.BLL.Specifications
{
    public interface ISpecification<T>  // هنا بعرف الحاجات ال ممكن استخدمها 
    {
        public Expression<Func<T, bool>> Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; } // p => p.Name  p => p.Price 

        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public bool IsPaginationEnable { get; set; }


    }
}
