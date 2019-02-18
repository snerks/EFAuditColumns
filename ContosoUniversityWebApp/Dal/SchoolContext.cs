using ContosoUniversityWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ContosoUniversityWebApp.Dal
{
    public interface ISystemClock
    {
        DateTimeOffset GetDateTimeOffsetUtcNow();
        DateTime GetUtcNow();
    }

    public class SystemClockAdapter : ISystemClock
    {
        public DateTimeOffset GetDateTimeOffsetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }

    // enable-migrations
    // add-migration InitialCreate
    // update-database
    public class SchoolContext : DbContext
    {
        public ISystemClock SystemClock { get; }

        //public SchoolContext() : base("SchoolContext")
        //{
        //}

        public SchoolContext() : this(new SystemClockAdapter())
        {
        }

        public SchoolContext(ISystemClock systemClock) : base("SchoolContext")
        {
            if (systemClock == null)
                throw new ArgumentNullException(nameof(systemClock));

            SystemClock = systemClock;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
              .Where(p => p.State == EntityState.Added)
              .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);

            var utcNow = SystemClock.GetDateTimeOffsetUtcNow();

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedAt = utcNow;
                added.LastModifiedAt = utcNow;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.LastModifiedAt = utcNow;
            }

            return base.SaveChanges();
        }
    }

    public interface IAuditedEntity
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string LastModifiedBy { get; set; }
        DateTimeOffset LastModifiedAt { get; set; }
    }
}