using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _Dbcontext;
        public CategoryController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _Dbcontext.Categories.Where(c => c.IsDeleted == false).ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var existCategory = await _Dbcontext.Categories.Where(c => !c.IsDeleted).ToListAsync();

            if (existCategory.Any(c => c.Name.ToLower().Equals(model.Name.ToLower())))
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            
            var newCategory = new Category
            {
                Name = model.Name
            };

            await _Dbcontext.Categories.AddAsync(newCategory);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var category = await _Dbcontext.Categories.FindAsync(id);
            
            if (category.id != id) return BadRequest();
            var existCategory = new CategoryUpdateModel
            {
                Id = category.id,
                Name = category.Name
            };
            return View(existCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }
            var category = await _Dbcontext.Categories.FindAsync(id);

            if (category is null) return NotFound();
            var isExistName = await _Dbcontext.Categories.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.id != id);

            if (isExistName)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View(model);
            }
            category.Name = model.Name;

            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var category = await _Dbcontext.Categories.FindAsync(id);

            if (category is null) return NotFound();

            _Dbcontext.Categories.Remove(category);

            await _Dbcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}