using BackFinalEdu.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class CourseCreateModel
    {
        public List<SelectListItem>? Categories { get; set; }

        public string? Image { get; set; }
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AboutCourse { get; set; }
        public string ApplyDetails { get; set; }
        public string SertificationContent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesments { get; set; }
    }
}
