namespace BackFinalEdu.DAL.Entities
{
    public class Speaker : Entity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}