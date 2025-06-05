using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Client
{
    public class BlogPostController : BaseController
    {
        private readonly IBlogPostService _blogPostService;
        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginateDatas([FromQuery] int page, [FromQuery] int take)
        {
            return Ok(await _blogPostService.GetPaginateAsync(page, take));
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            return Ok(await _blogPostService.SearchByCategoryAndName(name));
        }
    }
}
