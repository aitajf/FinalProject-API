using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.UI.Subscription;
using Service.Services;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Client
{
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Subscribe([FromBody] SubscriptionCreateDto request)
        //{
        //    var success = await _subscriptionService.SubscribeAsync(request.Email);

        //    if (!success)
        //        return BadRequest("This mail subscribe is exist");

        //    return Ok("Subscription success!");
        //}

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionCreateDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { Message = "Email is required !" });
            var message = await _subscriptionService.SubscribeAsync(request.Email);
            return Ok(new { Message = message });
        }
    }
}
