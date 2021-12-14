using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class StudentCourse
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
