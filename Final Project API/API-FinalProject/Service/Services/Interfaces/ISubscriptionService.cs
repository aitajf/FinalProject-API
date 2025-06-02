using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Service.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<string> SubscribeAsync(string email);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<bool> UnsubscribeAsync(string email);
    }
}
