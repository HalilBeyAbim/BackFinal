using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _Dbcontext;

        public TeacherController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var teachers = await _Dbcontext.Teachers.Where(t => !t.IsDeleted).ToListAsync();
            return View(teachers);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var teacher = await _Dbcontext.Teachers.SingleOrDefaultAsync(x => x.id == id);
            if (teacher.id != id) return BadRequest();
            return View(teacher);
        }
    }
}