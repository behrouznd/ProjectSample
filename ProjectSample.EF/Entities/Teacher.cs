using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectSample.EF.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }

        [InverseProperty("OnlineTeacher")]
        public ICollection<Course> OnlineCourses { get; set; }

        [InverseProperty("ClassRoomTeacher")]
        public ICollection<Course> ClassRoomCources { get; set; }
    }
}
