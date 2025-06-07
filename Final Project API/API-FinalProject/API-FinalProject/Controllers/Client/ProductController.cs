using API_FinalProject.Controllers.Client;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace FinalProject.Controllers.UI
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _productService.GetAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> ProductDetail([FromRoute] int id)
		{
			var product = await _productService.DetailAsync(id);

			if (product is null) return NotFound();

			return Ok(product);
		}

		[HttpGet]
		public async Task<IActionResult> SearchByName([FromQuery] string name)
		{
			return Ok(await _productService.SearchByCategoryAndName(name));
		}


        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] string? categoryName,[FromQuery] string? colorName,
                                                [FromQuery] string? tagName, [FromQuery] string? brandName)
        {
            return Ok(await _productService.FilterAsync(categoryName, colorName, tagName, brandName));
        }


        [HttpGet("take")]
        public async Task<IActionResult> GetAllTaken([FromQuery] int take, [FromQuery] int? skip = null)
        {
            var products = await _productService.GetAllTakenAsync(take, skip);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsCount()
        {
            var count = await _productService.GetProductsCountAsync();
            return Ok(count);
        }


        //[HttpGet("{id}/colors")]
        //public async Task<IActionResult> GetProductColors(int id)
        //{
        //    var productColorsDto = await _productService.GetProductColorsWithImagesAsync(id);

        //    if (productColorsDto == null)
        //        return NotFound();

        //    return Ok(productColorsDto);
        //}
    }
}
