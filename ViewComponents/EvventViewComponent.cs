using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.ViewComponents
{
    public class EvventViewComponent : ViewComponent
    {
        private readonly AppDbContext _Dbcontext;
        public EvventViewComponent(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _Dbcontext.Events.Where(e => !e.IsDeleted).ToListAsync();
            return View(events);
        }
    }
}