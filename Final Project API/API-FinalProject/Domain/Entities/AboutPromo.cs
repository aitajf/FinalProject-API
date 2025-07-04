﻿using Domain.Common;

namespace Domain.Entities
{
    public class AboutPromo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
