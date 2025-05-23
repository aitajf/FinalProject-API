using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Service.DTO.Admin.BlogPost
{
    public class BlogPostEditDto
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public string HighlightText { get; set; }
        public int BlogCategoryId { get; set; }
        //public List<string> ExistingImages { get; set; }      
        public ICollection<BlogPostImg> Images { get; set; }
        public List<IFormFile>? NewImages { get; set; }
    }
}
