using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.DAL.Entities
{
    public class Blog : Entity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}
