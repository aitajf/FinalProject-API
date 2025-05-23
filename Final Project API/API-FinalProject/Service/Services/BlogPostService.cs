using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTO.Admin.BlogPost;
using Service.Services;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _postRepository;
        private readonly IBlogCategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public BlogPostService(IBlogPostRepository postRepository,
                               IBlogCategoryRepository blogCategoryRepository,
                               IWebHostEnvironment env,
                               IFileService fileService,
                               IMapper mapper)
        {
            _postRepository = postRepository;
            _categoryRepository = blogCategoryRepository;
            _env = env;
            _fileService = fileService; 
            _mapper = mapper;
        }

        public async Task CreateAsync(BlogPostCreateDto model)
        {

            var categoryExists = await _categoryRepository.GetByIdAsync(model.BlogCategoryId);
            if (categoryExists == null)
            {
                throw new KeyNotFoundException($"Category with ID {model.BlogCategoryId} not found.");
            }
            var blogPost = _mapper.Map<BlogPost>(model);
            await _postRepository.CreateAsync(blogPost);
            if (model.Images != null && model.Images.Any())
            {
                blogPost.Images = new List<BlogPostImg>();

                foreach (var file in model.Images)
                {
                    string imageUrl = await _fileService.UploadFileAsync(file, "blogposts");
                    blogPost.Images.Add(new BlogPostImg { Image = imageUrl, BlogPostId = blogPost.Id });
                }
                await _postRepository.EditAsync(blogPost);
            }
        }

        public async Task EditAsync(int id, BlogPostEditDto model)
        {
            var blogPost = await _postRepository.GetByIdWithIncludesAsync(id);
            if (blogPost == null) throw new KeyNotFoundException($"Blog post with ID {id} not found.");

            var categoryExists = await _categoryRepository.GetByIdAsync(model.BlogCategoryId);
            if (categoryExists == null) throw new KeyNotFoundException($"Category with ID {model.BlogCategoryId} not found.");

            _mapper.Map(model, blogPost);
            blogPost.Images ??= new List<BlogPostImg>();

            if (model.Images != null && model.Images.Any())
            {
                foreach (var file in model.Images)
                {
                    string imageUrl = await _fileService.UploadFileAsync(file, "blogposts");
                    blogPost.Images.Add(new BlogPostImg { Image = imageUrl, BlogPostId = blogPost.Id, IsMain = false });
                }
            }

            //if (!string.IsNullOrEmpty(model.MainImageUrl) && blogPost.Images.Any(img => img.ImageUrl == model.MainImageUrl))
            //{
            //    foreach (var img in blogPost.Images)
            //    {
            //        img.IsMain = img.ImageUrl == model.MainImageUrl;
            //    }
            //}

            await _postRepository.EditAsync(blogPost);
        }

        public async Task DeleteAsync(int id)
        {
            var blogPost = await _postRepository.GetByIdWithIncludesAsync(id);
            if (blogPost == null) throw new KeyNotFoundException($"Blog post with ID {id} not found.");

            await _postRepository.DeleteAsync(blogPost);
        }

        public async Task<IEnumerable<BlogPostDto>> GetAllAsync()
        {
            var blogPosts = await _postRepository.ApplyIncludes().ToListAsync();
            return _mapper.Map<IEnumerable<BlogPostDto>>(blogPosts);
        }

        public async Task<BlogPostDto> GetByIdAsync(int id)
        {
            var blogPost = await _postRepository.ApplyIncludes().FirstOrDefaultAsync(bp => bp.Id == id); 
            if (blogPost == null) throw new KeyNotFoundException($"Blog post with ID {id} not found.");
            return _mapper.Map<BlogPostDto>(blogPost);
        }

        public async Task DeleteImageAsync(int blogPostId, int blogPostImageId)
        {
            var blogPost = await _postRepository.GetByIdWithIncludesAsync(blogPostId);
            if (blogPost == null) throw new KeyNotFoundException("Blog post not found.");

            var image = blogPost.Images.FirstOrDefault(i => i.Id == blogPostImageId);
            if (image == null) throw new KeyNotFoundException("Image not found.");

            string imageName = Path.GetFileName(image.Image);
            string filePath = Path.Combine(_env.WebRootPath, "Uploads", "blogposts", imageName);

            if (File.Exists(filePath))  _fileService.Delete(imageName, "blogposts");

            blogPost.Images.Remove(image);
            await _postRepository.EditAsync(blogPost);
        }
    }
}

