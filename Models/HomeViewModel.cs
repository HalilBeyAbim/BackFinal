using BackFinalEdu.DAL.Entities;

namespace BackFinalEdu.Models
{
    public class HomeViewModel
    {
        public List<Slider> sliders = new List<Slider>();
        public List<Course> Courses = new List<Course>();
        public List<Blog> Blogs = new List<Blog>();
        public List<Event> Events = new List<Event>();
    }
}