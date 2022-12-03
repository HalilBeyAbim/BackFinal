namespace BackFinalEdu.Areas.Admin.Models
{
    public class SlideImageUpdateModel
    {
        public int Id { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonLink { get; set; }
        public string ButtonText { get; set; }
    }
}
