using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTOs.UI.Wishlists;

namespace Service.Services.Interfaces
{
    public interface IWishlistService
    {
        Task AddWishlistAsync(WishlistDto wishlistDto);
        Task DeleteWishlistAsync(int id);
        Task DeleteProductFromWishList(int productId, int wishListId);
        Task<WishlistDto> GetByUserIdAsync(string userId);
    }
}
