using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTO.Admin.BlogCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IMapper _mapper;
        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepository,
                                IMapper mapper)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(BlogCategoryCreateDto model)
        {
            await _blogCategoryRepository.CreateAsync(_mapper.Map<BlogCategory>(model));
        }

        public async Task DeleteAsync(int id)
        {
            var askUs = await _blogCategoryRepository.GetByIdAsync(id);
            if (askUs == null) throw new KeyNotFoundException($"Category with ID {id} not found.");
            await _blogCategoryRepository.DeleteAsync(askUs);

        }

        public async Task EditAsync(BlogCategoryEditDto model, int id)
        {
            var existingCategory = await _blogCategoryRepository.GetByIdAsync(id);
            if (existingCategory == null) throw new KeyNotFoundException($"Category with ID {id} not found.");
            _mapper.Map(model, existingCategory);
            await _blogCategoryRepository.EditAsync(existingCategory);
        }

        public async Task<IEnumerable<BlogCategoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<BlogCategoryDto>>(await _blogCategoryRepository.GetAllAsync());
        }

        public async Task<BlogCategoryDto> GetByIdAsync(int id)
        {
            var category = await _blogCategoryRepository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Category with ID {id} not found.");
            return _mapper.Map<BlogCategoryDto>(category);
        }
    }
}
