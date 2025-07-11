﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories.Interface
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        Task<bool> RemoveAsync(string email);
        Task<Subscription> GetByEmailAsync(string email);
    }
}
