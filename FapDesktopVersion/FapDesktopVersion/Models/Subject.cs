using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Courses = new HashSet<Course>();
            Gradetitles = new HashSet<Gradetitle>();
        }

        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Gradetitle> Gradetitles { get; set; }
    }
}
