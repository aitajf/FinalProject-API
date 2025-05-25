using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTOs.UI.Wishlists;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductService _productService;

        public WishlistService(IWishlistRepository wishlistRepository, IProductService productService)
        {
            _wishlistRepository = wishlistRepository;
            _productService = productService;
        }

        public async Task<WishlistDto> GetByUserIdAsync(string userId)
        {
            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
            if (wishlist == null) return null;

            return new WishlistDto
            {
                AppUserId = wishlist.AppUserId,
                ProductId = wishlist.WishlistProducts.ToList().Count > 0 ? wishlist.WishlistProducts.ToList()[0].ProductId : 0 
            };
        }

        public async Task AddWishlistAsync(WishlistDto wishlistDto)
        {
            WishlistDto existWishListDto = await GetByUserIdAsync(wishlistDto.AppUserId);
            if (existWishListDto == null)
            {
                List<WishlistProduct> wishlistProducts = new List<WishlistProduct>
            {
                new WishlistProduct { ProductId = wishlistDto.ProductId }
            };
                var wishlist = new Wishlist
                {
                    AppUserId = wishlistDto.AppUserId,
                    WishlistProducts = wishlistProducts
                };

                ArgumentNullException.ThrowIfNull(nameof(AppUser.Id));

                var wishList = await _wishlistRepository.GetByUserIdAsync(wishlistDto.AppUserId) ?? throw new KeyNotFoundException("User not found");
                await _wishlistRepository.AddAsync(wishlist);
            }
            else
            {
                Wishlist existWishList = await _wishlistRepository.GetByUserIdAsync(existWishListDto.AppUserId);
                existWishList.WishlistProducts.Add(new WishlistProduct { ProductId = wishlistDto.ProductId });
            }
        }

        public async Task DeleteProductFromWishList(int productId, int wishListId)
        {
            Wishlist existWishList = await _wishlistRepository.GetByIdAsync(wishListId);
            existWishList.WishlistProducts.RemoveAll(wp => wp.ProductId == productId);
        }

        public async Task DeleteWishlistAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            var wishList = await _wishlistRepository.GetByIdAsync((int)id) ?? throw new KeyNotFoundException("Data not found.");
            await _wishlistRepository.DeleteAsync(id);
        }
    }
}
