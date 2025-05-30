using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interface;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Products;
using Service.Helpers;
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

        public async Task<ProductDetailDto> DetailAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var product = await _productRepository.GetByIdWithIncludesAsync(id) ?? throw new KeyNotFoundException($"Data with ID{id} not found");

            return _mapper.Map<ProductDetailDto>(product);
        }

        public async Task EditAsync(int id, ProductEditDto model)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            var exist = await _productRepository.GetAllWithExpressionAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (exist.ToList().Count > 0) throw new ArgumentException("This product has already exist");

            var product = await _productRepository.GetByIdWithIncludesAsync(id) ?? throw new KeyNotFoundException("Data not found");
            
            List<ProductImage> images = new();

            foreach (var item in model.UploadImages)
            {
                string fileUrl = await _fileService.UploadFileAsync(item, "productimages");
                images.Add(new ProductImage { Img = fileUrl });
            }

            if (images.Any())
            {
                images.FirstOrDefault().IsMain = true;
            }

            model.ProductImages = images;
            foreach (var item in model.ColorIds)
            {
                if (!product.ProductColors.Any(pc => pc.ColorId == item))
                {
                    var color = await _colorRepository.GetByIdAsync(item);
                    if (color == null)
                    {
                        throw new KeyNotFoundException("Color not found");
                    }
                    product.ProductColors.Add(new ProductColor { ColorId = item, Product = product });
                }
            }

            var colorsToRemove = product.ProductColors.Where(pc => !model.ColorIds.Contains(pc.ColorId)).ToList();
            foreach (var color in colorsToRemove)
            {
                product.ProductColors.Remove(color);
            }

            foreach (var item in model.TagIds)
            {
                if (!product.ProductTags.Any(pt => pt.TagId == item))
                {
                    var tag = await _tagRepository.GetByIdAsync(item);
                    if (tag == null)
                    {
                        throw new KeyNotFoundException("Tag not found");
                    }
                    product.ProductTags.Add(new ProductTag { TagId = item, Product = product });
                }
            }

            var tagsToRemove = product.ProductTags.Where(pt => !model.TagIds.Contains(pt.TagId)).ToList();
            foreach (var tag in tagsToRemove)
            {
                product.ProductTags.Remove(tag);
            }

            _mapper.Map(model, product);
            await _productRepository.EditAsync(product);
        }



        public async Task<IEnumerable<ProductDto>> FilterAsync(string categoryName, string colorName, string tagName, string brandName)
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.FilterAsync(categoryName, colorName, tagName, brandName));

        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ProductDto>>(_productRepository.GetAllWithIncludes());
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id);
            var product = await _productRepository.GetByIdWithIncludesAsync(id) ?? throw new KeyNotFoundException($"Data with ID{id} not found");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> SearchByCategoryAndName(string categoryOrProductName)
        {
            var products = _productRepository.GetAllWithExpression(x =>
                x.Name.ToLower().Trim().Contains(categoryOrProductName.ToLower().Trim()) 
             || x.Category.Name.ToLower().Trim().Contains(categoryOrProductName.ToLower().Trim()));
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SortBy(string sortKey)
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.SortBy(sortKey));
        }

        //public async Task<PaginationResponse<ProductDto>> GetPaginateAsync(int page, int take)
        //{
        //    var products = _productRepository.GetAllWithExpression(null);
        //    int totalItemCount = products.Count();
        //    int totalPage = (int)Math.Ceiling((decimal)totalItemCount / take);
        //    var mappedDatas = _mapper.Map<IEnumerable<ProductDto>>(products.Skip((page - 1) * take).Take(take).ToList());
        //    //return new PaginationResponse<ProductDto>(mappedDatas, totalPage, page, totalItemCount);
        //    return new PaginationResponse<ProductDto>(mappedDatas.ToList(), totalItemCount, page, take);

        //}

        public async Task<PaginationResponse<ProductDto>> GetPaginateAsync(int page, int take)
        {
            var products = _productRepository.GetAllWithExpression(null);
            int totalItemCount = products.Count();

            var paginated = products
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var mappedDatas = _mapper.Map<IEnumerable<ProductDto>>(paginated);

            int totalPage = (int)Math.Ceiling((decimal)totalItemCount / take);

            return new PaginationResponse<ProductDto>(mappedDatas, totalPage, page, totalItemCount);
        }
    }
}
