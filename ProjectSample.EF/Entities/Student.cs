using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSample.EF.Entities
{
    public class Student
    {
        public int StudentID { get; set; }

        [ConcurrencyCheck]
        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public int GradeId { get; set; }
        public Grade Grade { get; set; }


        public virtual StudentAddress Address { get; set; }


        public virtual ICollection<Course> Courses { get; set; }
    }

}
