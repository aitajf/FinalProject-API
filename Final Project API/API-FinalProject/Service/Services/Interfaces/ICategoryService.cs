﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO.Admin.Category;
using Service.DTO.Admin.Sliders;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateDto model);
        Task EditAsync(CategoryEditDto model, int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
    }
}
