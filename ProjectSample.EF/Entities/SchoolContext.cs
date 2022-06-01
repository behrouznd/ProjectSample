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
            // Configure a One-to-Many Relationship using Fluent API
            modelBuilder.Entity<Student>()
                .HasRequired<Grade>(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey<int>(s => s.GradeId)
                .WillCascadeOnDelete(); // automatically deleting child rows when the related parent row is deleted

            // Alternatively:
            modelBuilder.Entity<Grade>()
                .HasMany<Student>(s => s.Students)
                .WithRequired(s => s.Grade)
                .HasForeignKey<int>(s => s.GradeId);

            //---------------------------------------------------------------------
            //Configure a Many-to-Many Relationship
            modelBuilder.Entity<Student>()
                .HasMany<Course>(s => s.Courses)
                .WithMany(c => c.Students)
                .Map(cs =>
                {
                    cs.MapLeftKey("StudentRefId");
                    cs.MapRightKey("CourdeRefId");
                    cs.ToTable("StudentCourse");
                });

            //---------------------------------------------------------------------
            //CUD Operations using Stored Procedures
            modelBuilder.Entity<Student>().MapToStoredProcedures();

            modelBuilder.Entity<Teacher>().MapToStoredProcedures(
                p => p.Insert(sp => sp.HasName("sp_InsertTeacher").Parameter(pm => pm.Name, "name").Result(rs => rs.TeacherId, "id"))
                .Update(sp => sp.HasName("sp_UpdateTeacher").Parameter(pm => pm.Name, "name"))
                .Delete(sp => sp.HasName("sp_DeleteTeacher").Parameter(pm => pm.Name, "name"))
                );
            //---------------------------------------------------------------------
            // Move Fluent API Configurations to a Separate Class
            modelBuilder.Configurations.Add(new TeacherEntityConfiguration());
            //------------------------------------------------
            //Define Custom Code - First Conventions
            modelBuilder.Properties().Where(p => p.PropertyType.Name == "String").Configure(p => p.HasMaxLength(50));


            
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

            //--Seed Data
            IList<Grade> defaultGrades = new List<Grade>();
            defaultGrades.Add(new Grade() { GradeName = "Grade 1", Section = "Section 1" });
            defaultGrades.Add(new Grade() { GradeName = "Grade 2", Section = "Section 2" });
            context.Grades.AddRange(defaultGrades);
            //-----------

            base.Seed(context);
        }
    }
}
