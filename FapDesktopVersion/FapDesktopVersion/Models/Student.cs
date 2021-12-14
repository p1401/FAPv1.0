using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
            RollCallBooks = new HashSet<RollCallBook>();
            StudentCourses = new HashSet<StudentCourse>();
        }

        public int StudentId { get; set; }
        public string Roll { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<RollCallBook> RollCallBooks { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
