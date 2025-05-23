using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.Admin.BlogCategory;
using Service.DTO.Admin.BlogPost;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Admin
{
    public class BlogPostController : BaseController
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogPostCreateDto model)
        {
             await _blogPostService.CreateAsync(model);
             return CreatedAtAction(nameof(Create), "Blog Post created success.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromForm] BlogPostEditDto model)
        {
            await _blogPostService.EditAsync(id, model);
            return Ok(new { message = "Blog post updated successfully!" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogPostService.DeleteAsync(id);
            return Ok(new { message = "Blog post deleted successfully!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blogPostService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _blogPostService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int blogPostId, int blogPostImageId)
        {
            await _blogPostService.DeleteImageAsync(blogPostId, blogPostImageId);

            return Ok(new { message = "Image deleted successfully." });
        }
    }
}
