using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleAppCore.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string YearBirth { get; private set; }
    }
}
