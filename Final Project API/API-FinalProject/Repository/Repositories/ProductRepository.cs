﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

       
        public async Task<IEnumerable<Product>> FilterAsync(string? categoryName, string? colorName, string? tagName, string? brandName)
        {
            var query = GetAllWithIncludes();

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.Name.ToLower() == categoryName.ToLower());
            }

            if (!string.IsNullOrEmpty(colorName))
            {
                query = query.Where(p => p.ProductColors.Any(pc => pc.Color.Name.ToLower() == colorName.ToLower()));
            }

            if (!string.IsNullOrEmpty(tagName))
            {
                query = query.Where(p => p.ProductTags.Any(pt => pt.Tag.Name.ToLower() == tagName.ToLower()));
            }

            if (!string.IsNullOrEmpty(brandName))
            {
                query = query.Where(p => p.Brand.Name.ToLower() == brandName.ToLower());
            }

            return await query.ToListAsync();
        }

        public IQueryable<Product> GetAllWithExpression(Expression<Func<Product, bool>> predicate)
        {
            predicate ??= x => true;
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
  
        public async Task<IEnumerable<Product>> GetAllTakenAsync(int take, int? skip = null)
        {
            return await _context.Products.Include(m => m.Category)
                                          .Include(m => m.Brand)
                                          .Include(m => m.ProductImages)
                                          .Include(m => m.ProductColors)
                                          .ThenInclude(pc => pc.Color)
                                          .Include(m => m.ProductTags)
                                          .ThenInclude(ps => ps.Tag).Skip(skip ?? 0)
                                          .Take(take).ToListAsync();
        }

        public async Task<int> GetProductsCount()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<Product> GetProductWithColorsAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> SortedProductsAsync(string sortType)
        {
            var query = _context.Products.Include(p => p.ProductImages) 
                                         .AsQueryable();

            switch (sortType)
            {
                case "A-Z":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "Z-A":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "Latest":
                    query = query.OrderByDescending(p => p.CreatedDate);
                    break;
                case "PriceLowToHigh":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "PriceHighToLow":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name); 
                    break;
            }

            return await query.ToListAsync();
        }

        //public IEnumerable<Product> GetRandomProductsByCategory(int categoryId, int count = 3)
        //{
        //    return _context.Products
        //                   .Where(p => p.CategoryId == categoryId)
        //                   .Include(p => p.Category) 
        //                   .Include(p => p.Brand)
        //                   .Include(m => m.ProductColors)
        //                   .ThenInclude(pc => pc.Color)
        //                   .Include(p => p.ProductImages)   
        //                   .OrderBy(r => Guid.NewGuid())
        //                   .Take(count).ToList();
        //}

        public IEnumerable<Product> GetComparisonProducts(int categoryId, int selectedProductId, int count = 3)
        {
            // 1. Seçilmiş məhsulu gətir
            var selectedProduct = _context.Products
                       .Include(p => p.Category)
                       .Include(p => p.Brand)
                       .Include(m => m.ProductColors)
                       .ThenInclude(pc => pc.Color)
                       .Include(p => p.ProductImages)   
                .FirstOrDefault(p => p.Id == selectedProductId);

            if (selectedProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {selectedProductId} not found!");
            }

            var randomProducts = _context.Products
                .Where(p => p.CategoryId == categoryId && p.Id != selectedProductId) 
                .OrderBy(r => Guid.NewGuid()) 
                .Take(count - 1) 
                .ToList();

            randomProducts.Insert(0, selectedProduct);

            return randomProducts;
        }


    }
}