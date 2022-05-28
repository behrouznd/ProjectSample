using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
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
            ////modelBuilder.HasDefaultSchema("admin");

            ////modelBuilder.Entity<Teacher>().ToTable("Teacher", "dbo");

            //modelBuilder.Entity<Student>().Map(m =>
            //{
            //    m.Properties(p => new { p.StudentID, p.StudentName });
            //    m.ToTable("StudentInfo");
            //}).Map(m => {
            //    m.Properties(p => new { p.StudentID, p.Height, p.Weight, p.Photo, p.DateOfBirth });
            //    m.ToTable("StudentInfoDetail");
            //});

            //--------------------------------------------------------------
            modelBuilder.Entity<Student>().Map(delegate (EntityMappingConfiguration<Student> studentConfig)
                {
                    studentConfig.Properties(p => new { p.StudentID, p.StudentName });
                    studentConfig.ToTable("StudentInfo");
                }
            );

            Action<EntityMappingConfiguration<Student>> studentMapping = m =>
            {
                m.Properties(p => new { p.StudentID, p.Height, p.Weight, p.Photo, p.DateOfBirth });
                m.ToTable("StudentInfoDetail");
            };

            modelBuilder.Entity<BillingDetail>()
                .Map<BankAccount>(m => m.Requires("BillingDetailType").HasValue("BA"))
                .Map<CreditCard>(m => m.Requires("BillingDetailType").HasValue("CC"));

            modelBuilder.Entity<Student>().Map(studentMapping);
            //---------------------------------------------------------------------
            modelBuilder.Entity<Student>().HasKey<int>(s => s.StudentID);
            //---------------------------------------------------------------------
            modelBuilder.Entity<Student>()
                .Property(p => p.DateOfBirth)
                .HasColumnName("DoB")
                .HasColumnOrder(3)
                .HasColumnType("datatime2")
                .IsOptional();

            modelBuilder.Entity<Student>()
                .Property(p => p.Weight)
                .IsRequired();

            //IsFixedLength() change datatype from nvarchar to nchar
            modelBuilder.Entity<Student>()
                .Property(p => p.StudentName)
                .HasMaxLength(50)
                .IsFixedLength();

            modelBuilder.Entity<Student>()
                .Property(p => p.Height)
                .HasPrecision(2, 2);

            modelBuilder.Entity<Student>()
                .Property(p => p.StudentName)
                .IsConcurrencyToken();

            modelBuilder.Entity<Student>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
            //---------------------------------------------------------------------
            //Configure a One-to-Zero-or-One relationship using Fluent API
            modelBuilder.Entity<Student>()
                .HasOptional(s => s.Address) // Mark Address property optional in Student entity
                .WithRequired(s => s.Student);// mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student
                
                // .WithRequiredPrincipal(ad => ad.Student); // One-to-One relationship

            //---------------------------------------------------------------------
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


            //modelBuilder.Entity<Course>().Property(s => s.CreatedDate).HasDefaultValueSql("GETDATE()");
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
