﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.Admin.BlogPost
{
    public class BlogPostImgDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
    }
}
