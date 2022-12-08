using System.ComponentModel.DataAnnotations;

namespace BackFinalEdu.DAL.Entities
{
    public class Event : Entity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}