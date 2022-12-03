using Microsoft.AspNetCore.Mvc;

namespace BackFinalEdu.Areas.Admin.Models
{
    public class UpdateTeacherModel
    {
        public int Id { get; set; }

        public string Image { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Pinterest { get; set; }
        public string Dribbble { get; set; }
        public byte Language { get; set; }
        public byte TeamLeader { get; set; }
        public byte Devolopment { get; set; }
        public byte Design { get; set; }
        public byte Innovation { get; set; }
        public byte Communication { get; set; }
    }
}
