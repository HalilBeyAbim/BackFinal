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
    public class EvventController : BaseController
    {
        private readonly AppDbContext _Dbcontext;
        public EvventController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _Dbcontext.Events
            .Include(e => e.EventSpeakers)
            .ThenInclude(e => e.Speaker)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(e => e.id)
            .ToListAsync();
             return View(events);
        }
        public async Task<IActionResult> Create()
        {
            var speaker = await _Dbcontext.Speakers.ToListAsync();
            var eventSpeakersListItem = new List<SelectListItem>();
            speaker.ForEach(s => eventSpeakersListItem.Add(new SelectListItem(s.Name, s.id.ToString())));
            var model = new EventCreateViewModel
            {
                Speakers = eventSpeakersListItem
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartDate) >= 0)
            {
                ModelState.AddModelError("StartTime", "Start Date must be in future, before finish time..!");
                return View(model);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndDate) >= 0)
            {
                ModelState.AddModelError("EndTime", "End Time must be after StartTime..!");
                return View(model);
            }
            if (DateTime.Compare(model.StartDate, model.EndDate) >= 0)
            {
                ModelState.AddModelError("", "Start time must be before End Time ..!");
                return View(model);
            }

            if (!model.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "An image must be selected.!");
                return View(model);
            }

            if (!model.Photo.IsAllowedSize(20))
            {
                ModelState.AddModelError("Image", "Image size can be maximum 20mb..!");
                return View(model);
            }
            var unicalFileName = await model.Photo.Generatefile(Constants.EventPath);
            var createdEvent = new Event
            {
                Image = unicalFileName,
                Title = model.Title,
                Content = model.Content,
                Location = model.Location,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = "test"
            };
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (var speakerId in model.SpeakerIds)
            {
                if (!await _Dbcontext.Speakers.AnyAsync(s => s.id == speakerId))
                {
                    ModelState.AddModelError("", "such a speaker");
                }
                
                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId
                });
            }
            var speakers = await _Dbcontext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var speakersSelectListItem = new List<SelectListItem>();
            speakers.ForEach(s => speakersSelectListItem.Add(new SelectListItem(s.Name, s.id.ToString())));
            createdEvent.EventSpeakers = eventSpeakers;
            model.Speakers = speakersSelectListItem;
            await _Dbcontext.Events.AddAsync(createdEvent);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var eventt = await _Dbcontext.Events
            .Include(e => e.EventSpeakers)
            .ThenInclude(e => e.Speaker)
            .Where(e => !e.IsDeleted && e.id == id)
            .FirstOrDefaultAsync();

            if (eventt is null) return NotFound();
            var speakers = await _Dbcontext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var eventSpeakerListItem = new List<SelectListItem>();
            speakers.ForEach(s => eventSpeakerListItem.Add(new SelectListItem(s.Name, s.id.ToString())));
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (EventSpeaker eSpeaker in eventt.EventSpeakers)
            {
                if (!await _Dbcontext.Speakers.AnyAsync(s => s.id == eSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "such a speaker");
                    return View();
                }
                
                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = eSpeaker.id
                });
            }
            var eventUpdateViewModel = new EventUpdateViewModel
            {
                Title = eventt.Title,
                Content = eventt.Content,
                Image = eventt.Image,
                Location = eventt.Location,
                Speakers = eventSpeakerListItem,
                SpeakerIds = eventSpeakers.Select(s => s.SpeakerId).ToList(),
                StartDate = eventt.StartDate,
                EndDate = eventt.EndDate,
            };
            return View(eventUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var eventt = await _Dbcontext.Events
            .Include(e => e.EventSpeakers)
            .ThenInclude(e => e.Speaker)
            .Where(e => !e.IsDeleted && e.id == id)
            .FirstOrDefaultAsync();

            if (eventt is null) return NotFound();


            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartDate) >= 0)
            {
                ModelState.AddModelError("StartTime", "Başlama tarixi gələcəkdə olmalıdı, lakin bitmə vaxtından öncə olmalıdı..!");
                return View(model);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndDate) >= 0)
            {
                ModelState.AddModelError("EndTime", "Bitiş vaxtı başlama vaxtından sonran olmalıdı..!");
                return View(model);
            }
            
            if (DateTime.Compare(model.StartDate, model.EndDate) >= 0)
            {
                ModelState.AddModelError("", "Başlama tarixi bitmə tarixindən əvvəl olmalıdı..!");
                return View(model);
            }
            
            var speakers = await _Dbcontext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var speakerList = new List<SelectListItem>();
            speakers.ForEach(s => speakerList.Add(new SelectListItem(s.Name, s.id.ToString())));
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            
            if (model.SpeakerIds.Count > 0)
            {
                foreach (int speakerId in model.SpeakerIds)
                {
                    if (!await _Dbcontext.Speakers.AnyAsync(s => s.id == speakerId))
                    {
                        ModelState.AddModelError("", "Yanlış spiker seçdiniz..!");
                        return View(model);
                    }
                    
                    eventSpeakers.Add(new EventSpeaker
                    {
                        SpeakerId = speakerId
                    });
                }
                eventt.EventSpeakers = eventSpeakers;
            }
            
            else
            {
                ModelState.AddModelError("", "Ən azı 1 spiker seçilməlidir..!");
                return View(model);
            }

            if (model.Photo is not null)
            {
                if (!model.Photo.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(model);
                }

                if (!model.Photo.IsAllowedSize(20))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(model);
                }
                var unicalFileName = await model.Photo.Generatefile(Constants.EventPath);
                eventt.Image = unicalFileName;
            }
            var viewModel = new EventUpdateViewModel
            {
                Speakers = speakerList
            };
            if (!ModelState.IsValid) return View(viewModel);
            eventt.Title = model.Title;
            eventt.Content = model.Content;
            eventt.Location = model.Location;
            eventt.StartDate = model.StartDate;
            eventt.EndDate = model.EndDate;
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var existedEvent = await _Dbcontext.Events.FindAsync(id);
            
            if (existedEvent is null) return NotFound();
            
            if (existedEvent.id != id) return NotFound();
            var eventImage = Path.Combine(Constants.RootPath, "assets", "img", "event", existedEvent.Image);
            
            if (System.IO.File.Exists(eventImage))
                System.IO.File.Delete(eventImage);
            _Dbcontext.Events.Remove(existedEvent);
            await _Dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}