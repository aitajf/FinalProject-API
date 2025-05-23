using Service.DTO.Admin.Tag;

namespace Service.Services.Interfaces
{
    public interface ITagService
    {
        Task CreateAsync(TagCreateDto model);
        Task EditAsync(TagEditDto model, int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto> GetByIdAsync(int id);
    }
}
