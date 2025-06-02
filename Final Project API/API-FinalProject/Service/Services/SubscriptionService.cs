using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAccountService _accountService;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IAccountService accountService)
        {
            _subscriptionRepository = subscriptionRepository;
            _accountService = accountService;
        }

        //public async Task<bool> SubscribeAsync(string email)
        //{
        //    var existingSubscription = await _subscriptionRepository.GetByEmailAsync(email);
        //    if (existingSubscription != null) return false; 

        //    var existingUser = await _accountService.GetUserByEmailAsync(email);
        //    if (existingUser == null) return false;

        //    var subscription = new Subscription
        //    {
        //        Email = email,
        //        CreatedDate = DateTime.UtcNow
        //    };

        //    await _subscriptionRepository.CreateAsync(subscription);
        //    return true;
        //}


        public async Task<string> SubscribeAsync(string email)
        {
            var existingUser = await _accountService.GetUserByEmailAsync(email);

            if (existingUser == null)
                return "First be register.";

            var existingSubscription = await _subscriptionRepository.GetByEmailAsync(email);

            if (existingSubscription != null)
                return "Subscribe with this mail exist!";

            var subscription = new Subscription
            {
                Email = email,
                CreatedDate = DateTime.UtcNow
            };

            await _subscriptionRepository.CreateAsync(subscription);
            return "Subscribe succesfully !";
        }


        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _subscriptionRepository.GetAllAsync();
        }

        public async Task<bool> UnsubscribeAsync(string email)
        {
            return await _subscriptionRepository.RemoveAsync(email);
        }
    }
}
