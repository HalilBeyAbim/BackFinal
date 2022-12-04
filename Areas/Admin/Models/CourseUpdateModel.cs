using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class CourseUpdateModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Photo { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string AboutCourse { get; set; }
        public string HowToApply { get; set; }
        public string Certification { get; set; }
        public string? StartDate { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public string StudentCount { get; set; }
        public string Assesments { get; set; }
        public List<SelectListItem>? Categories { get; internal set; }
    }
}
