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
            var homeViewModel = new HomeViewModel { 
            sliders =  Sliders           
            };
            return View(homeViewModel);
        }
    }
}