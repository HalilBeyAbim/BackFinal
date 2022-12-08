using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly AppDbContext _Dbcontext;
        public CourseViewComponent(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _Dbcontext.Courses.Where(x => !x.IsDeleted).Include(c => c.Category).ToListAsync();
            return View(courses);
        }
    }
}