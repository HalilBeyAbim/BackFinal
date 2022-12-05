using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
