﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.UI.BlogReview
{
    public class BlogReviewCreateDto
    {
        public int PostId { get; set; }
        public string AppUserId { get; set; }
        public string Comment { get; set; }
    }
}
