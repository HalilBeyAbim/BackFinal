using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly AppDbContext _Dbcontext;
        public TeacherViewComponent(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teacher = await _Dbcontext.Teachers.Where(t => !t.IsDeleted).ToListAsync();
            return View(teacher);
        }
    }
}