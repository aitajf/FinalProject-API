using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTO.Admin.BlogCategory;
using Service.DTO.Admin.Tag;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository,
                                IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(TagCreateDto model)
        {
            await _tagRepository.CreateAsync(_mapper.Map<Tag>(model));
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) throw new KeyNotFoundException($"Tag with ID {id} not found.");
            await _tagRepository.DeleteAsync(tag);

        }

        public async Task EditAsync(TagEditDto model, int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) throw new KeyNotFoundException($"Tag with ID {id} not found.");
            _mapper.Map(model, tag);
            await _tagRepository.EditAsync(tag);
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<TagDto>>(await _tagRepository.GetAllAsync());
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) throw new KeyNotFoundException($"Tag with ID {id} not found.");
            return _mapper.Map<TagDto>(tag);
        }
    }
}
