
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Repository.Repositories.Interface;
using Service.DTOs.UI.Basket;

namespace Service.Services
{
    public class BasketService : IBasketService
    {
		private readonly IBasketRepository _basketRepository;
		private readonly IColorRepository _colorRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public BasketService(IBasketRepository basketRepository,
							 IColorRepository colorRepository,
							 IHttpContextAccessor httpContextAccessor)
		{
			_basketRepository = basketRepository;
            _colorRepository = colorRepository;
			_httpContextAccessor = httpContextAccessor;
		}

        public async Task<BasketDto> GetBasketByUserIdAsync(string userId)
        {
            var basket = await _basketRepository.GetByUserIdAsync(userId);
            if (basket == null) return null;

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            return new BasketDto
            {
                AppUserId = basket.AppUserId,
                BasketProducts = basket.BasketProducts.Select(x => new BasketProductDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    ColorId = x.ColorId,
                    ColorName = x.Color.Name,
                    Price = x.Product.Price,
                    ProductImage = $"{Path.Combine(baseUrl, "productimages", x.Product.ProductImages.FirstOrDefault()?.Img)}"
                }).ToList(),
                TotalProductCount = basket.BasketProducts.Sum(x => x.Quantity),
                TotalPrice = basket.BasketProducts.Sum(x => x.Product.Price * x.Quantity)
            };
        }

        public async Task DeleteProductByUserIdAsync(string userId)
        {
             await _basketRepository.DeleteProductByUserIdAsync(userId);
        }

            public async Task AddBasketAsync(BasketCreateDto basketCreateDto)
		{
			if (string.IsNullOrEmpty(basketCreateDto.UserId) || basketCreateDto.ProductId == 0)
			{
				throw new ArgumentNullException("UserId or ProductId cannot be null or zero.");
			}
			else if (!await _colorRepository.GetWithIncludes(c => c.Id == basketCreateDto.ColorId && c.ProductColors.Any(p => p.ProductId == basketCreateDto.ProductId)))
			{
				throw new KeyNotFoundException("color id  not found");
			}
			var existBasket = await _basketRepository.GetByUserIdAsync(basketCreateDto.UserId);

			if (existBasket == null)
			{
				existBasket = new Basket
				{
					AppUserId = basketCreateDto.UserId
				};
				existBasket.BasketProducts.Add(new BasketProduct { ProductId = basketCreateDto.ProductId, Quantity = 1, ColorId = basketCreateDto.ColorId });
				await _basketRepository.AddAsync(existBasket);
			}
			else
			{
				var existingProduct = existBasket.BasketProducts.FirstOrDefault(x => x.ProductId == basketCreateDto.ProductId);
				if (existingProduct == null || existBasket.BasketProducts.Any( x=>x.ColorId != basketCreateDto.ColorId))
				{
					existBasket.BasketProducts.Add(new BasketProduct { ProductId = basketCreateDto.ProductId, Quantity = 1, ColorId = basketCreateDto.ColorId });
				}
				else
				{
					existingProduct.Quantity++;
				}
			}
		}

		public async Task IncreaseQuantityAsync(BasketCreateDto basketCreateDto)
        {
            if (string.IsNullOrEmpty(basketCreateDto.UserId) || basketCreateDto.ProductId == 0)
            {
                throw new ArgumentNullException("UserId or ProductId cannot be null or zero.");
            }

            var basket = await _basketRepository.GetByUserIdAsync(basketCreateDto.UserId);

            if (basket == null)
            {
                throw new KeyNotFoundException("Basket not found for the given user.");
            }

            var product = basket.BasketProducts.FirstOrDefault(bp => bp.ProductId == basketCreateDto.ProductId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found in the basket.");
            }
            if (product != null)
            {
                product.Quantity++;
            }
        }

        public async Task DecreaseQuantityAsync(BasketCreateDto basketCreateDto)
        {

            if (string.IsNullOrEmpty(basketCreateDto.UserId) || basketCreateDto.ProductId == 0)
            {
                throw new ArgumentNullException("UserId or ProductId cannot be null or zero.");
            }

            var basket = await _basketRepository.GetByUserIdAsync(basketCreateDto.UserId);

            if (basket == null)
            {
                throw new KeyNotFoundException("Basket not found for the given user.");
            }

            var product = basket.BasketProducts.FirstOrDefault(bp => bp.ProductId == basketCreateDto.ProductId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found in the basket.");
            }

            product.Quantity--;

            if (product.Quantity == 0)
            {
                basket.BasketProducts.Remove(product);
            }
        }

		public async Task DeleteProductFromBasketAsync(int productId, string userId)
        {
            if (string.IsNullOrEmpty(userId) || productId == 0)
            {
                throw new ArgumentNullException("UserId or ProductId cannot be null or zero.");
            }
            var basket = await _basketRepository.GetByUserIdAsync(userId);
            if (basket == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            if (basket != null)
            {
                var product = basket.BasketProducts.FirstOrDefault(bp => bp.ProductId == productId);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found");
                }
                if (product != null)
                {
                    basket.BasketProducts.Remove(product);
                }
            }
        }

        public async Task<List<BasketDto>> GetAllBasketsAsync()
        {
            var baskets = await _basketRepository.FindAllWithIncludes()
                .Include(x => x.BasketProducts)
                .ThenInclude(x => x.Color)
                .Include(x => x.BasketProducts)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .ToListAsync();

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            return baskets.Select(basket => new BasketDto
            {
                AppUserId = basket.AppUserId,
                BasketProducts = basket.BasketProducts.Select(x => new BasketProductDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    ColorId = x.ColorId,
                    ColorName = x.Color.Name,
                    Price = x.Product.Price,
                    ProductImage = $"{Path.Combine(baseUrl, "productimages", x.Product.ProductImages.FirstOrDefault()?.Img)}"
                }).ToList(),
                TotalProductCount = basket.BasketProducts.Sum(x => x.Quantity),
                TotalPrice = basket.BasketProducts.Sum(x => x.Product.Price * x.Quantity)
            }).ToList();
        }
    }
}
