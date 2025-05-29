using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Repository.Repositories.Interface;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Products;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,
                              ICategoryRepository categoryRepository,
                              IBrandRepository brandRepository,
                              IColorRepository colorRepository,
                              ITagRepository tagRepository,
                              IProductTagRepository productTagRepository,
                              IProductColorRepository productColorRepository,
                              IFileService fileService,
                              IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _colorRepository = colorRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _productColorRepository = productColorRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductCreateDto model)
        {
            if (model is null) throw new ArgumentNullException();
            var exist = await _productRepository.GetAllWithExpressionAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (exist.ToList().Count > 0) throw new ArgumentException("This product has already exist");

            List<ProductImage> images = new();
            foreach (var item in model.Images)
            {
                string fileUrl = await _fileService.UploadFileAsync(item, "productimages");
                images.Add(new ProductImage { Img = fileUrl });
            }

            images.FirstOrDefault().IsMain = true;
            model.ProductImages = images;

            var data = _mapper.Map<Product>(model);

            if (await _brandRepository.GetByIdAsync(model.BrandId) is null) throw new KeyNotFoundException("Brand not found");

            if (await _categoryRepository.GetByIdAsync(model.CategoryId) is null) throw new KeyNotFoundException("Category not found");

            await _productRepository.CreateAsync(data);

            foreach (int id in model.TagIds)
            {
                if (await _tagRepository.GetByIdAsync(id) == null) throw new KeyNotFoundException($"Tag with ID {id} not found.");
                await _productTagRepository.CreateAsync(new ProductTag { TagId = id, Product = data });
            }

            foreach (var id in model.ColorIds)
            {
                if (await _colorRepository.GetByIdAsync(id) == null)  throw new KeyNotFoundException($"Color with ID {id} not found.");
                await _productColorRepository.CreateAsync(new ProductColor { ColorId = id, Product = data });
            }
        }


        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(id);
            if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found.");
            await _productRepository.DeleteAsync(product);
        }

        public Task DeleteImageAsync(int productId, int productImageId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDetailDto> DetailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(int id, ProductEditDto model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> FilterAsync(string categoryName, string colorName, string tagName, string brandName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ProductDto>>(_productRepository.GetAllWithIncludes());
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDto>> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> SortBy(string sortKey)
        {
            throw new NotImplementedException();
        }
    }
}
