using System.Linq.Expressions;
using Domain.Entities;

namespace Repository.Repositories.Interface
{
    public interface IBlogPostRepository : IBaseRepository<BlogPost>
    {
        IQueryable<BlogPost> ApplyIncludes();
        Task<BlogPost> GetByIdWithIncludesAsync(int id);
        IQueryable<BlogPost> GetAllWithExpression(Expression<Func<BlogPost, bool>> predicate);
        Task<IEnumerable<BlogPost>> FilterAsync(string categoryName);

    }
}
