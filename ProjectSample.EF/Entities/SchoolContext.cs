using System;
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
    }

    public class SchoolDbInitializer : CreateDatabaseIfNotExists<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            base.Seed(context);
        }
    }
}
