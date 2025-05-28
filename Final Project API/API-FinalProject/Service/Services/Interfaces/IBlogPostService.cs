using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO.Admin.BlogPost;

namespace Service.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task CreateAsync(BlogPostCreateDto model);
        Task EditAsync(int id, BlogPostEditDto model);
        Task DeleteAsync(int id);
        Task<IEnumerable<BlogPostDto>> GetAllAsync();
        Task<BlogPostDto> GetByIdAsync(int id);

        Task<bool> DeleteImageAsync(int productId, int productImageId);
    }
}
