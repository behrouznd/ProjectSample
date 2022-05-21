using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSample.EF.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        [MaxLength(60)]
        public string CourseName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey("OnlineTeacher")]
        public int? OnlineTeacherId { get; set; }
        public Teacher OnlineTeacher { get; set; }

        [ForeignKey("ClassRoomTeacher")]
        public int? ClassRoomTeacherId { get; set; }
        public Teacher ClassRoomTeacher { get; set; }
    }
}
