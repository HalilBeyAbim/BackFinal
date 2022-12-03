namespace BackFinalEdu.Areas.Admin.Models
{
    public class SlideImageCreateModel
    {
        public IFormFile Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonLink { get; set; }
        public string ButtonText { get; set; }
    }
}
