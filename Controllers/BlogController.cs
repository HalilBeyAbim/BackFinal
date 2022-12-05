using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
