﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTO.Admin.Brand;
using Service.DTO.Admin.Category;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public BrandService(IBrandRepository brandRepository,
                             IFileService fileService,
                             IMapper mapper)

        {
            _brandRepository = brandRepository;
            _fileService = fileService;
            _mapper = mapper;
        }
        public async Task CreateAsync(BrandCreateDto model)
        {
            string imageUrl = await _fileService.UploadFileAsync(model.Image, "brands");
            Brand brand = _mapper.Map<Brand>(model);
            brand.Image = imageUrl;
            await _brandRepository.CreateAsync(brand);
        }

        public async Task DeleteAsync(int id)
        {
            Brand brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) throw new KeyNotFoundException($"Brand with ID {id} not found.");

            if (!string.IsNullOrEmpty(brand.Image))
            {
                string fileName = Path.GetFileName(brand.Image);
                _fileService.Delete(fileName, "brands");
            }
            await _brandRepository.DeleteAsync(brand);
        }

        public async Task EditAsync(BrandEditDto model, int id)
        {
            Brand brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null) throw new KeyNotFoundException($"Brand with ID {id} not found.");

            if (model.Image != null)
            {
                string oldFileName = Path.GetFileName(brand.Image);
                _fileService.Delete(oldFileName, "brands");
                string newImage = await _fileService.UploadFileAsync(model.Image, "brands");
                brand.Image = newImage;
            }
            _mapper.Map(model, brand);
            brand.Image = brand.Image;
            await _brandRepository.EditAsync(brand);
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<BrandDto>>(await _brandRepository.GetAllAsync());
        }

        public async Task<BrandDto> GetByIdAsync(int id)
        {
            var category = await _brandRepository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Brand with ID {id} not found.");
            return _mapper.Map<BrandDto>(category);
        }
    }
}
