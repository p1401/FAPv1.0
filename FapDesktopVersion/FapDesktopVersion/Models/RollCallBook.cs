using System;
using System.Collections.Generic;

#nullable disable

namespace FapDesktopVersion.Models
{
    public partial class RollCallBook
    {
        public int RollCallBookId { get; set; }
        public int? TeachingScheduleId { get; set; }
        public int? StudentId { get; set; }
        public bool? IsAbsent { get; set; }

        public bool isAbsent
        {
            set => IsAbsent = value;
            get => IsAbsent ?? throw new InvalidOperationException("Uninitialized property: " + nameof(IsAbsent));
        }

        public string Comment { get; set; }

        public virtual Student Student { get; set; }
        public virtual CourseSchedule TeachingSchedule { get; set; }
    }
}
