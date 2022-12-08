using BackFinalEdu.DAL;
using BackFinalEdu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;
        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.Where(x => !x.IsDeleted).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var blogViewModel = new BlogViewModel
            {
                Blogs = blogs,
                Categories = categories
            };
            return View(blogViewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.id == id);
            
            if (blog is null) return NotFound();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var blogViewModel = new BlogViewModel
            {
                Blog = blog,
                Categories = categories,
            };
            return View(blogViewModel);
        }
    }
}