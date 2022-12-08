using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class SliderImageController : BaseController
    {
        private readonly AppDbContext _DbContext;
        public SliderImageController(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliderImages = await _DbContext.Sliders.ToListAsync();
            return View(sliderImages);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideImageCreateModel model)
        {
            if (!ModelState.IsValid) return View();

            if (!model.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            
            if (!model.Photo.IsAllowedSize(10))
            {
                ModelState.AddModelError("Photo", "Max size 2mb");
                return View();
            }

            var unicalName = await model.Photo.Generatefile(Constants.SliderPath);
            var slider = new Slider
            {
                Image = unicalName,
                Title = model.Title,
                Description = model.Description,
                ButtonLink = model.ButtonLink,
                ButtonText = model.ButtonText

            };
            await _DbContext.Sliders.AddAsync(slider);
            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var slider = await _DbContext.Sliders
                .Where(sl => sl.id == id)
                .FirstOrDefaultAsync();

            if (slider is null) return NotFound();
            var sliderViewModel = new SlideImageUpdateModel
            {
                Id = slider.id,
                Title = slider.Title,
                Description = slider.Description,
                ButtonLink = slider.ButtonLink,
                ButtonText = slider.ButtonText,
                Image = slider.Image
            };

            return View(sliderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideImageUpdateModel model)
        {
            if (id is null) return BadRequest();
            var slider = await _DbContext.Sliders
                .Where(sl => sl.id == id)
                .FirstOrDefaultAsync();

            if (slider is null) return NotFound();

            if (!ModelState.IsValid) return View(new SlideImageUpdateModel
            {
                Image = slider.Image
            });

            if (model.Photo != null)
            {
                if (!model.Photo.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new SlideImageUpdateModel
                    {
                        Image = slider.Image,
                    });
                }

                if (!model.Photo.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (slider.Image is null) return NotFound();

                var sliderImagePath = Path.Combine(Constants.RootPath, "assets", "img", "slider", slider.Image);

                if (System.IO.File.Exists(sliderImagePath))
                    System.IO.File.Delete(sliderImagePath);

                var unicalName = await model.Photo.Generatefile(Constants.SliderPath);
                slider.Image = unicalName;
            }
            slider.Title = model.Title;
            slider.Description = model.Description;
            slider.ButtonText = model.ButtonText;
            slider.ButtonLink = model.ButtonLink;
            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _DbContext.Sliders.FirstOrDefaultAsync(s => s.id == id);

            if (slider == null) return NotFound();

            if (slider.Image == null) return NotFound();

            if (slider.id != id) return BadRequest();

            var sliderImagePath = Path.Combine(Constants.RootPath, "assets", "img", "slider", slider.Image);

            if (System.IO.File.Exists(sliderImagePath))
                System.IO.File.Delete(sliderImagePath);

            _DbContext.Sliders.Remove(slider);

            await _DbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}