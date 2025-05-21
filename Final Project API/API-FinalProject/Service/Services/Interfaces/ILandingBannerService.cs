using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO.Admin.LandingBanner;
using Service.DTO.Admin.Sliders;

namespace Service.Services.Interfaces
{
    public interface ILandingBannerService
    {
        Task CreateAsync(LandingBannerCreateDto model);
        Task EditAsync(LandingBannerEditDto model, int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<LandingBannerDto>> GetAllAsync();
        Task<LandingBannerDto> GetByIdAsync(int id);
    }
}
