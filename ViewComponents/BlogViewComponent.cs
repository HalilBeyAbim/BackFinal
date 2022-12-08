using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _Dbcontext;
        public BlogViewComponent(AppDbContext dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _Dbcontext.Blogs.Where(x => !x.IsDeleted).ToListAsync();    

            return View(blogs);
        }
    }
}