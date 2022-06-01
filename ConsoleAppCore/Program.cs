using ConsoleAppCore.Entities;
using System;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //--Access Shadow Property--
            using (var context   = new SchoolContext())
            {
                var std = new Student() { Name = "Bill" };
                context.Entry(std).Property("CreatedDate").CurrentValue = DateTime.Now;
                var createdTime = context.Entry(std).Property("CreatedDate").CurrentValue;
            }
            //----    


            Console.WriteLine("Hello World!");
        }
    }
}
