using ContosoUniversityWebApp.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversityWebApp.Models
{
    public class Student : IAuditedEntity
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
    }
}