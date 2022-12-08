using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class EventCreateViewModel
    {
        public IFormFile Photo { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public List<SelectListItem>? Speakers { get; set; }
        public List<int> SpeakerIds { get; set; }
    }
}