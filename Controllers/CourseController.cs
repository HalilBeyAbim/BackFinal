using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
