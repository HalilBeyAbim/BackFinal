using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Controllers
{
    public class EvventController : Controller
    {
        private readonly AppDbContext _Dbcontext;
        public EvventController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return NotFound();
            var events = await _Dbcontext.Events.Where(c => !c.IsDeleted && c.id == id).Include(e => e.EventSpeakers).ThenInclude(e => e.Speaker).FirstOrDefaultAsync();
            if (events is null) return NotFound();
            return View(events);
        }
    }
}