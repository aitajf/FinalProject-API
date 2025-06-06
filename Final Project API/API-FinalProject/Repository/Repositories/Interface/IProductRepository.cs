

using Domain.Entities;
using Repository.Repositories.Interface;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> GetAllWithIncludes();
        IQueryable<Product> GetAllWithExpression(Expression<Func<Product, bool>> predicate);
        Task<Product> GetByIdWithIncludesAsync(int id);
        Task<IEnumerable<Product>> FilterAsync(string categoryName,string colorName, string tagName, string brandName);
        Task<IEnumerable<Product>> SortBy(string sortKey);
		Task DeleteProductImage(ProductImage image);
        Task<IEnumerable<Product>> GetAllTakenAsync(int take, int? skip = null);
        Task<int> GetProductsCount();
        Task<Product> GetProductWithColorsAsync(int productId);
    }
}
