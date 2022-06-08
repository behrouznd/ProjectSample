using ConsoleAppCore.Entities;
using System;
using System.Linq;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (var context   = new SchoolContext())
            {
                //--Access Shadow Property--
                var std = new Student() { Name = "Bill" };
                context.Entry(std).Property("CreatedDate").CurrentValue = DateTime.Now;
                var createdTime = context.Entry(std).Property("CreatedDate").CurrentValue;
                //----   


                //--ChangeTracker.TrackGraph()--
                context.ChangeTracker.TrackGraph(std, e =>
               {
                   if (e.Entry.IsKeySet)
                   {
                       e.Entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                   }
                   else
                   {
                       e.Entry.State = Microsoft.EntityFrameworkCore.EntityState.Added;
                   }
               });
                //----


                var aaa=  context.Students.Where(x => x.StudentId == DbFunction.MyFunction()).ToList();
            }
             


            Console.WriteLine("Hello World!");
        }
    }
}
