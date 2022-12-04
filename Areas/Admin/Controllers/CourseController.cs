using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        private readonly AppDbContext _Dbcontext;
        public CourseController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _Dbcontext.Courses.Where(c => !c.IsDeleted).Include(c => c.Category).ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _Dbcontext.Categories.Where(c => !c.IsDeleted).ToListAsync();

            var categoryListItem = new List<SelectListItem>
            {
                new SelectListItem("Select Category" , "0")
            };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.id.ToString())));
            var model = new CourseCreateModel
            {
                Categories = categoryListItem
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateModel model)
        {
            var categories = await _Dbcontext.Categories.Where(c => !c.IsDeleted).Include(c => c.Courses).ToListAsync();
            if (!ModelState.IsValid) return View(model);

            var categoryListItem = new List<SelectListItem>
             {
                 new SelectListItem("Parent Category Secin", "0")
             };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.id.ToString())));

            var viewModel = new CourseCreateModel
            {
                Categories = categoryListItem
            };
            var createdCourse = new Course();

            if (!model.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "Picture must be choosing..!");
                return View(model);
            }

            if (!model.Photo.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Image can contain max 10mb ..!");
                return View(model);
            }

            var unicalName = await model.Photo.Generatefile(Constants.CoursePath);

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Please select category");
                return View(model);
            }

            createdCourse.ApplyDetails = model.ApplyDetails;
            createdCourse.AboutCourse = model.AboutCourse;
            createdCourse.Description = model.Description;
            createdCourse.CategoryId = model.CategoryId;
            createdCourse.Image = unicalName;
            createdCourse.SkillLevel = model.SkillLevel;
            createdCourse.Duration = model.Duration;
            createdCourse.ClassDuration = model.ClassDuration;
            createdCourse.Language = model.Language;
            createdCourse.Duration = model.Duration;
            createdCourse.StudentCount = model.StudentCount;
            createdCourse.Assesments = model.Assesments;
            createdCourse.SertificationContent = model.SertificationContent;
            createdCourse.Title = model.Title;
            createdCourse.StartTime = model.StartTime;
            createdCourse.EndTime = model.EndTime;
            createdCourse.CategoryId = model.CategoryId;

            await _Dbcontext.Courses.AddAsync(createdCourse);
                await _Dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var category = await _Dbcontext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (category is null) return NotFound();
            var course = await _Dbcontext.Courses.Where(c => !c.IsDeleted && c.id == id).Include(c => c.Category).FirstOrDefaultAsync();
            if (course is null) return NotFound();

            if (course.id != id) return BadRequest();

            var selectedCategories = new List<SelectListItem>();

            category.ForEach(c => selectedCategories.Add(new SelectListItem(c.Name, c.id.ToString())));
            var courseUpdateViewModel = new CourseUpdateModel
            {
                Id = course.id,
                AboutCourse = course.AboutCourse,
                Description = course.Description,
                CategoryId = course.CategoryId,
                Photo = course.Image,
                SkillLevel = course.SkillLevel,
                Duration = course.Duration,
                ClassDuration = course.ClassDuration,
                Language = course.Language,
                Categories = selectedCategories,



            };
            return View(courseUpdateViewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id is null) return NotFound();
            var categories = await _Dbcontext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (categories is null) return NotFound();
            var course = await _Dbcontext.Courses.Where(c => !c.IsDeleted && c.id == id).Include(c => c.Category).FirstOrDefaultAsync();
            if (course is null) return NotFound();
            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new CourseUpdateModel
                    {
                        Photo = course.Image,
                    });
                }


                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Picture must be choosing..!");
                    return View(model);
                }


                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new CourseUpdateModel
                    {
                        Photo = course.Image
                    });
                }



                var path = Path.Combine(Constants.RootPath, "img", "course", course.Image);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.Generatefile(Constants.CoursePath);
                course.Image = unicalName;
            }

                var selectedCategory = new CourseUpdateModel
                {
                    CategoryId = model.CategoryId,


                };
                course.Description = model.Description;
                course.ClassDuration = model.ClassDuration;
                course.Duration = model.Duration;
                course.Language = model.Language;
                course.CategoryId = model.CategoryId;
                course.SkillLevel = model.SkillLevel;
                course.AboutCourse = model.AboutCourse;
                course.CategoryId = model.CategoryId;
                await _Dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var course = await _Dbcontext.Courses
                .FirstOrDefaultAsync(x => x.id == id && !x.IsDeleted);

            if (course is null) return NotFound();

            if (course.id != id) return BadRequest();

            var path = Path.Combine(Constants.RootPath, "assets", "img", "course", course.Image);

            var result = System.IO.File.Exists(path);
            if (result)
            {
                System.IO.File.Delete(path);
            }
            _Dbcontext.Courses.Remove(course);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
