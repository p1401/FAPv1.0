using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class Grade
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GradeTitle { get; set; }
        public double? Grade1 { get; set; }

        public virtual Course Course { get; set; }
        public virtual Gradetitle GradeTitleNavigation { get; set; }
        public virtual Student Student { get; set; }
    }
}
