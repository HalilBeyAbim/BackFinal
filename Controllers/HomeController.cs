using BackFinalEdu.DAL;
using BackFinalEdu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackFinalEdu.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _Dbcontext;
        public HomeController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public IActionResult Index()
        {
            var Sliders = _Dbcontext.Sliders.ToList();
            var courses =  _Dbcontext.Courses.Where(x => !x.IsDeleted).ToList();
            var blogs = _Dbcontext.Blogs.Where(x => !x.IsDeleted).ToList();
            var homeViewModel = new HomeViewModel { 
            sliders =  Sliders,
                Courses = courses,
                Blogs = blogs
            };
            return View(homeViewModel);
        }
    }
}