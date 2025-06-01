using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Client
{
    public class BlogPostController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginateDatas([FromQuery] int page, [FromQuery] int take)
        {
            return Ok(await _blogPostService.GetPaginateAsync(page, take));

        }
    }
}
