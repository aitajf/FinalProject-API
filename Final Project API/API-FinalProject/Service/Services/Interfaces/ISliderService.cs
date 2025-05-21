using Service.DTO.Admin.Sliders;

namespace Service.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreateAsync(SliderCreateDto model);
        Task EditAsync(SliderEditDto model, int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<SliderDto>> GetAllAsync();
        Task<SliderDto> GetByIdAsync(int id);
    }
}
