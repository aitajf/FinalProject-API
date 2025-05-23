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
    public class BlogPostRepository : BaseRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(AppDbContext context) : base(context) { }

        public IQueryable<BlogPost> ApplyIncludes()
        {
            return _context.BlogPosts
                           .Include(x => x.Images) 
                           .Include(x => x.BlogCategory); 
        }
        public async Task<BlogPost> GetByIdWithIncludesAsync(int id)
        {
            return await _context.BlogPosts.Include(x => x.Images).Include(x => x.BlogCategory)
                                           .FirstOrDefaultAsync(x => x.Id == id); 
        }
    }
}
