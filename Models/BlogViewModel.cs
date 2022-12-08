using BackFinalEdu.DAL.Entities;

namespace BackFinalEdu.Models
{
    public class BlogViewModel
    {
        public List<Category> Categories { get; set; }
        public Blog Blog { get; set; }
        public List<Blog> Blogs { get; internal set; }
    }
}