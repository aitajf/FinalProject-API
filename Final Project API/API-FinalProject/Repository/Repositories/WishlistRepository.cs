using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interface;

namespace Repository.Repositories
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);

        }
        public async Task UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);

        }
        public async Task DeleteAsync(int id)
        {
            var wishlist = await GetByIdAsync(id);
            if (wishlist != null) _context.Wishlists.Remove(wishlist);
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            return await _context.Wishlists.Include(w => w.WishlistProducts).ThenInclude(wp => wp.Product)
                                           .ThenInclude(p => p.ProductImages).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Wishlist> GetByUserIdAsync(string userId)
        {
            return await _context.Wishlists.Include(w => w.WishlistProducts).ThenInclude(wp => wp.Product)
                                           .ThenInclude(p => p.ProductImages).FirstOrDefaultAsync(w => w.AppUserId == userId);
        }      
    }
}
