using BackFinalEdu.Areas.Admin.Data;
using BackFinalEdu.Areas.Admin.Models;
using BackFinalEdu.DAL;
using BackFinalEdu.DAL.Entities;
using BackFinalEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

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
            if (id == null)
            return NotFound();
 
            var teacher = await _Dbcontext.Teachers.FindAsync(id);
            return View(new UpdateTeacherModel
            {
                Image = teacher.Image,
                Name = teacher.Name,
                Profession = teacher.Profession,
                Degree = teacher.Degree,
                Description = teacher.Description,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Mail = teacher.Mail,
                Phone = teacher.Phone,
                Skype = teacher.Skype,
                Language = teacher.Language,
                TeamLeader = teacher.TeamLeader,
                Devolopment = teacher.Devolopment,
                Design = teacher.Design,
                Innovation = teacher.Innovation,
                Communication = teacher.Communication,
                Facebook = teacher.Facebook,
                Twitter = teacher.Twitter,
                Dribbble = teacher.Dribbble,
                Pinterest = teacher.Pinterest,
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, UpdateTeacherModel model)
        {
            if (id == null) return NotFound();
            var teacher = await _Dbcontext.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();
            
            if (teacher.id != id)
            return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(new UpdateTeacherModel
                {
                    Image = model.Image,
                });
            }

            if(model.Photo != null)
            {
            if (!model.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "Please select image type");
                return View(new UpdateTeacherModel
                {
                    Image = model.Image,
                });
            }
                if (!model.Photo.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }
                var teacherImagePath = Path.Combine(Constants.RootPath, "assets", "img", "slider", teacher.Image);
                
                if (System.IO.File.Exists(teacherImagePath))
                    System.IO.File.Delete(teacherImagePath);
                var unicalName = await model.Photo.Generatefile(Constants.TeacherPath);
                teacher.Image = unicalName;
            }
          
            teacher.Name = model.Name;
            teacher.Profession = model.Profession;
            teacher.Description = model.Description;
            teacher.Degree = model.Degree;
            teacher.Experience = model.Experience;
            teacher.Hobbies = model.Hobbies;
            teacher.Faculty = model.Faculty;
            teacher.Mail = model.Mail;
            teacher.Phone= model.Phone;
            teacher.Skype = model.Skype;
            teacher.Language = model.Language;
            teacher.TeamLeader = model.TeamLeader;
            teacher.Devolopment = model.Devolopment;
            teacher.Design = model.Design;
            teacher.Innovation = model.Innovation;
            teacher.Communication = model.Communication;
            teacher.Facebook = model.Facebook;
            teacher.Twitter = model.Twitter;
            teacher.Dribbble = model.Dribbble;
            teacher.Pinterest = model.Pinterest;
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _Dbcontext.Teachers.FindAsync(id);

            if (teacher.id == null) BadRequest();
            var path = Path.Combine(Constants.RootPath, "img", teacher.Image);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            _Dbcontext.Teachers.Remove(teacher);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}