﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSample.EF.Entities
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(): base()
        {
            Database.SetInitializer<SchoolContext>(new CreateDatabaseIfNotExists<SchoolContext>());

            //Disable Initializer
            //Database.SetInitializer<SchoolContext>(null);

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade>  Grades { get; set; }

        public DbSet<BillingDetail> BillingDetails { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingDetail>()
                .Map<BankAccount>(m => m.Requires("BillingDetailType").HasValue("BA"))
                .Map<CreditCard>(m => m.Requires("BillingDetailType").HasValue("CC"));

            //modelBuilder.Entity<BankAccount>()
            //    .Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //        m.ToTable("BankAccount");
            //    });

            //modelBuilder.Entity<CreditCard>()
            //    .Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //        m.ToTable("CreditCard");
            //    });

        }
    }

    public class SchoolDbInitializer : CreateDatabaseIfNotExists<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            base.Seed(context);
        }
    }
}
