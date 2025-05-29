

using Microsoft.AspNetCore.Http;
using Service.DTOs.Admin.Products;

namespace Service.Services.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductCreateDto model);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
		Task<ProductDetailDto> DetailAsync(int id);
		Task DeleteAsync(int id);
        Task EditAsync(int id, ProductEditDto model);
        Task<IEnumerable<ProductDto>> SearchByName(string name);
        Task<IEnumerable<ProductDto>> FilterAsync(string categoryName, string colorName, string tagName, string brandName);
        Task<IEnumerable<ProductDto>> SortBy(string sortKey);
        Task DeleteImageAsync(int productId, int productImageId);
	}
}