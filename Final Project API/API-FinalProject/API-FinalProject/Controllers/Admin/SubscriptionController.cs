using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.UI.Subscription;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Admin
{
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return Ok(subscriptions);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Unsubscribe([FromRoute]string email)
        {
            var success = await _subscriptionService.UnsubscribeAsync(email);
            return success ? Ok("Delete success.") : NotFound("Email not found.");
        }
    }
}
