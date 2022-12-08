using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly AppDbContext _Dbcontext;

        public BlogController(AppDbContext dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _Dbcontext.Blogs.ToListAsync();
            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBlogModel model)
        {
            if (!ModelState.IsValid) return View();
            
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Please select image type");
                return View();
            }
            
            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Max size 2mb");
                return View();
            }
            
            var unicalName = await model.Image.Generatefile(Constants.BlogPath);
            var blog = new Blog
            {
                Image = unicalName,
                Title = model.Title,
                Author = model.Author,
                Date = model.Date,
                Description = model.Description
            };
            await _Dbcontext.Blogs.AddAsync(blog);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _Dbcontext.Blogs.FindAsync(id);
            
            if (blog == null) return NotFound();
            var model = new UpdateBlogModel
            {
                Title = blog.Title,
                Author = blog.Author,
                Date = blog.Date,
                Description = blog.Description
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _Dbcontext.Blogs.FindAsync(id);

            if (blog.id == null) BadRequest();
            var path = Path.Combine(Constants.RootPath, "img", blog.Image);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _Dbcontext.Blogs.Remove(blog);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}