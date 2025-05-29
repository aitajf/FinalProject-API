using API_FinalProject.Controllers.Admin;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Products;
using Service.Services.Interfaces;

namespace FinalProject.Controllers.Admin
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto request)
        {
            await _productService.CreateAsync(request);

            return CreatedAtAction(nameof(Create), new { response = "Data successfully created" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id == null) return BadRequest();
            await _productService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductEditDto request)
        {
            await _productService.EditAsync(id, request);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int productId, int productImageId)
        {
            await _productService.DeleteImageAsync(productId, productImageId);

            return Ok(new { message = "Image deleted successfully." });
        }
	}
}
