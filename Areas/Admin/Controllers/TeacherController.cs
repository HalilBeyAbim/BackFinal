using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly AppDbContext _Dbcontext;

        public TeacherController(AppDbContext dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var teachers = await _Dbcontext.Teachers.ToListAsync();
            return View(teachers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherModel model)
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
            var unicalName = await model.Image.Generatefile(Constants.TeacherPath);
            var teacher = new Teacher
            {
                Image = unicalName,
                Name = model.Name,
                Degree = model.Degree,
                Description = model.Description,
                Facebook = model.Facebook,
                Twitter = model.Twitter,
                Dribbble = model.Dribbble,
                Experience = model.Experience,
                Faculty = model.Faculty,
                Hobbies = model.Hobbies,
                Mail = model.Mail,
                Phone = model.Phone,
                Profession = model.Profession,
                Skype = model.Skype,
                Pinterest = model.Pinterest,
                Language = model.Language,
                TeamLeader = model.TeamLeader,
                Devolopment = model.Devolopment,
                Design = model.Design,
                Innovation = model.Innovation,
                Communication = model.Communication
                
            };
            await _Dbcontext.Teachers.AddAsync(teacher);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _Dbcontext.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
    }
}

         

       
