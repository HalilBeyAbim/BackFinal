using BackFinalEdu.DAL.Entities;

namespace BackFinalEdu.Models
{
    public class CourseViewModel
    {
        public List<Blog> Blogs { get; set; }
        public Course Course { get; set; }
        public List<Category> Categories { get; set; }
    }
}