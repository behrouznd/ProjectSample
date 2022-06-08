using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAppCore.Entities
{



    public class SchoolContext : DbContext
    {
        //public static readonly ILoggerFactory consoleLoggerFactory
        //    = new LoggerFactory(new[] {
        //          new ConsoleLoggerProvider((category, level) =>
        //            category == DbLoggerCategory.Database.Command.Name &&
        //            level == LogLevel.Information, true)
        //        });


        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //--Shadow Property in Entity Framework Core--
            modelBuilder.Entity<Student>().Property<DateTime>("CreatedDate");
            modelBuilder.Entity<Student>().Property<DateTime>("UpdatedDate");
            //----

            //--Configuring Shadow Properties on All Entities --
            var allEntities = modelBuilder.Model.GetEntityTypes();
            foreach (var entity in allEntities)
            {
                entity.AddProperty("CreatedDate", typeof(DateTime));
                entity.AddProperty("UpdateDate", typeof(DateTime));
            }
            //----

            modelBuilder.HasDbFunction(() => DbFunction.MyFunction());
            //----
            modelBuilder.Entity<Student>().Property(c => c.YearBirth).HasComputedColumnSql("Datepart(yyyy,[BirthDate])");

            //---------
            modelBuilder.Entity<Student>().Property(c => c.BirthDate).HasValueGenerator(typeof(DateTimeValueGenerator));
            modelBuilder.Entity<Student>().Property(c => c.BirthDate).HasDefaultValueSql("getdate()");

            modelBuilder.HasSequence<int>("NewMySeq").HasMin(1000).IncrementsBy(5).IsCyclic(true);
            modelBuilder.Entity<Student>().Property(c => c.StudentId).HasDefaultValueSql("Next value for NewMySeq");
            //----
            modelBuilder.Entity<Student>().Property(c => c.Name).IsConcurrencyToken();

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            //--Access Shadow Property--
            var entities = ChangeTracker
                .Entries()
                .Where(s => s.State == EntityState.Added || s.State == EntityState.Modified);
            foreach (var entityEntry in entities)
            {
                entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }


    public static class DbFunction
    {
        public static int MyFunction()
        {
            throw new NotImplementedException();
        }
    }


    public class DateTimeValueGenerator : ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTime Next( Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            return DateTime.Now.AddYears(-10);
        }
    }
}
