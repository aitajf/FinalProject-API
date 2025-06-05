using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Service.DTO.UI.Review;

namespace Service.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetAllByProductIdAsync(int productId);
        Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId); 
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<bool> CreateReviewAsync(string userEmail, ReviewCreateDto dto); 
        Task<bool> UpdateReviewAsync(string userEmail, ReviewEditDto dto);   
        Task<bool> DeleteReviewAsync(string userEmail, int reviewId);        
        Task<bool> AdminDeleteReviewAsync(int reviewId);
    }
}
