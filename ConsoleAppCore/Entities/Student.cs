using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleAppCore.Entities
{
    public interface IAuditable
    {
        int UserIdAdded { get; set; }
        DateTime DatetimeAdded { get; set; }

    }


    public class Student : IAuditable
    {
        public int StudentId { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string YearBirth { get; private set; }
    }
}
