using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class UpdateBlogModel
        
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}