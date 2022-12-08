using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class CreateBlogModel 
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}