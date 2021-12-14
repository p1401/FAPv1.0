using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseSchedules = new HashSet<CourseSchedule>();
            Grades = new HashSet<Grade>();
            StudentCourses = new HashSet<StudentCourse>();
        }

        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseDescription { get; set; }
        public int SubjectId { get; set; }
        public int? InstructorId { get; set; }
        public int? TermId { get; set; }
        public int? CampusId { get; set; }

        public virtual Campus Campus { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Term Term { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
