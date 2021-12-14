using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class Gradetitle
    {
        public Gradetitle()
        {
            Grades = new HashSet<Grade>();
        }

        public int GradeTitleId { get; set; }
        public string GradeTitleName { get; set; }
        public string Description { get; set; }
        public byte Percent { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
