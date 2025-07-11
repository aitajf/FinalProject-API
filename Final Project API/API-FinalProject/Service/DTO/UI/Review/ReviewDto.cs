﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.UI.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }  
        public string AppUserId { get; set; }
        public string UserName { get; set; }  
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
