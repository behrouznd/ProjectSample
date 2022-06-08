using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppCore.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string YearBirth { get; private set; }
    }
}
