using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repository.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task DeleteProductImage(ProductImage image)
        {
            _context.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> FilterAsync(string categoryName, string colorName, string tagName, string brandName)
        {
            IEnumerable<Product> query = await GetAllWithIncludes().ToListAsync();

             List<Product> datas = new();
            if (categoryName == null && colorName == null && tagName == null && brandName == null)
            {
                return query.ToList();
            }
            else if (categoryName != null && colorName != null && tagName == null && brandName != null)
            {
                datas = query.Where(x => x.ProductColors.Any(x => x.Color.Name.ToLower() == colorName.ToLower()) ||
                        x.ProductTags.Any(x => x.Tag.Name.ToLower() == tagName.ToLower()) ||
                        x.Category.Name.ToLower() == categoryName.ToLower() ||
                        x.Brand.Name.ToLower() == brandName.ToLower()).ToList();
            }
            else
            {
                if (categoryName is not null)
                {
                    datas = query.Where(m => m.Category.Name == categoryName).ToList();
                }
                if (colorName is not null)
                {
                    datas = query.Where(m => m.ProductColors.Any(c => c.Color.Name == colorName)).ToList();
                }
                if (tagName is not null)
                {
                    datas = query.Where(m => m.ProductTags.Any(c => c.Tag.Name == colorName)).ToList();
                }
                if (brandName is not null)
                {
                    datas = query.Where(m => m.Brand.Name == brandName).ToList();
                }
            }
            return datas;
        }

        public IQueryable<Product> GetAllWithExpression(Expression<Func<Product, bool>> predicate)
        {
            return _context.Products
              .Include(m => m.Category)
              .Include(m => m.Brand)
              .Include(m => m.ProductImages)
              .Include(m => m.ProductColors)
              .ThenInclude(pc => pc.Color)
              .Include(m => m.ProductTags)
              .ThenInclude(ps => ps.Tag)
             .Where(predicate);
        }

        public IQueryable<Product> GetAllWithIncludes()
        {
            return _context.Products
              .Include(m => m.Category)
              .Include(m => m.Brand)
              .Include(m => m.ProductImages)
              .Include(m => m.ProductColors)
              .ThenInclude(pc => pc.Color)
              .Include(m => m.ProductTags)
              .ThenInclude(ps => ps.Tag);
        }

        public async Task<Product> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Products
               .Where(m => m.Id == id)
              .Include(m => m.Category)
              .Include(m => m.Brand)
              .Include(m => m.ProductImages)
              .Include(m => m.ProductColors)
              .ThenInclude(pc => pc.Color)
              .Include(m => m.ProductTags)
              .ThenInclude(ps => ps.Tag)  
              .AsNoTracking()
              .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> SortBy(string sortKey)
        {
            List<Product> products = new();
            switch (sortKey)
            {
                case "Price":
                    products = await _context.Products.OrderByDescending(m => m.Price).ToListAsync();
                    break;               
                case "A-Z":
                    products = await _context.Products.OrderBy(m => m.Name).ToListAsync();
                    break;
                case "Z-A":
                    products = await _context.Products.OrderByDescending(m => m.Name).ToListAsync();
                    break;
                default:
                    products = await _context.Products.ToListAsync();
                    break;
            }
            return products;

        }
    }
}