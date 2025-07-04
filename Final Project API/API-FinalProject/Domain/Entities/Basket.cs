﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Basket : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }


        public Basket()
        {
            BasketProducts = new List<BasketProduct>();
        }
    }
}
