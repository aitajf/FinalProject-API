using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.Admin.AskUsFrom;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Client
{
    public class AskUsFromController : BaseController
    {
        private readonly IAskUsFromService _askUsFromService;
        public AskUsFromController(IAskUsFromService askUsFromService)
        {
            _askUsFromService = askUsFromService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AskUsFromCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(request);
            await _askUsFromService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), "Created success");
        }
    }
}
