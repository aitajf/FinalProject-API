using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO.Admin.PromoCode;

namespace Service.Services.Interfaces
{
    public interface IPromoCodeService
    {
        Task CreateAsync(PromoCodeCreateDto dto);
        Task<PromoCodeResultDto?> GetByCodeAsync(string code);
        Task<PromoCodeCheckResultDto> CheckAndApplyAsync(string code);
        Task<bool> IncrementUsageCountAsync(string code);
        Task<List<PromoCodeDto>> GetAllAsync();
    }
}
