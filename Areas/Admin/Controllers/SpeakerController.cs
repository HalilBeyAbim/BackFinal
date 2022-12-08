using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Data;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class SpeakerController : BaseController
    {
        private readonly AppDbContext _dbContext;
        public SpeakerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var speakers = await _dbContext.Speakers.Where(t => !t.IsDeleted).ToListAsync();
            return View(speakers);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var speakers = await _dbContext.Speakers.Where(t => !t.IsDeleted && t.id == id).FirstOrDefaultAsync();

            return View(speakers);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            return View(model);

            if (!model.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "Image must choose..!");
                return View(model);
            }

            if (!model.Photo.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Image can contain max 10 mb..!");
                return View(model);
            }

            var unicalFileName = await model.Photo.Generatefile(Constants.EventPath);
            var speaker = new Speaker
            {
                Image = unicalFileName,
                Name = model.Name,
                Profession = model.Profession,
                Company = model.Company
            };
            await _dbContext.Speakers.AddAsync(speaker);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var speaker = await _dbContext.Speakers.FindAsync(id);
            
            if (speaker is null) return BadRequest();
            var speakerUpdateViewModel = new SpeakerUpdateViewModel
            
            {
                Image = speaker.Image,
                Name = speaker.Name,
                Profession = speaker.Profession,
                Company = speaker.Company
            };
            return View(speakerUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateViewModel model)
        {
            if (id is null) return NotFound();
            var speaker = await _dbContext.Speakers.Where(x => x.id == id).FirstOrDefaultAsync();

            if (speaker is null) return NotFound();

            if (speaker.id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new SpeakerUpdateViewModel
                    {
                        Image = speaker.Image,
                    });
                }

                if (!model.Photo.IsImage())
                {
                    ModelState.AddModelError("Image", "Please Choose Image Formats..!");
                    return View(new SpeakerUpdateViewModel
                    {
                        Image = speaker.Image,
                    });
                }

                if (!model.Photo.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new SpeakerUpdateViewModel
                    {
                        Image = speaker.Image,
                    });
                }

                var path = Path.Combine(Constants.RootPath, "img", "event", speaker.Image);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Photo.Generatefile(Constants.SpeakerPath);
                speaker.Image = unicalFileName;
            }
            
            speaker.Name = model.Name;
            speaker.Profession = model.Profession;
            speaker.Company = model.Company;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var speaker = await _dbContext.Speakers.Where(x => x.id == id).FirstOrDefaultAsync();

            if (speaker is null) return NotFound();

            if (speaker.id != id) return BadRequest();
            var path = Path.Combine(Constants.RootPath, "img", "event", speaker.Image);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}