using BackFinalEdu.DAL;
using BackFinalEdu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbContext;
        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var course = await _dbContext.Courses.Where(x => !x.IsDeleted && x.id == id).FirstOrDefaultAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted && x.id == id).Include(c => c.Courses).ToListAsync();
            var blog = await _dbContext.Blogs.Where(b => !b.IsDeleted).OrderByDescending(b => b.id).ToListAsync();
            var courseViewModel = new CourseViewModel
            {
                Course = course,
                Categories = categories,
                Blogs = blog
            };
            return View(courseViewModel);
        }
    }
}