using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}