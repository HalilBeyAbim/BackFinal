namespace BackFinalEdu.Areas.Admin.Models
{
    public class SpeakerCreateViewModel
    {
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
    }
}