using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackFinalEdu.DAL.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}