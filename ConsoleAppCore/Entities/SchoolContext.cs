using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAppCore.Entities
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Property<DateTime>("CreatedDate");
            modelBuilder.Entity<Student>().Property<DateTime>("UpdatedDate");

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
}
