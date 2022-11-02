using System;
using System.Collections.Generic;

#nullable disable

namespace LabLinqJoin22.Models
{
    public partial class Schedule
    {
        public int ScheduleId { get; set; }
        public int? CurriculumId { get; set; }
        public DateTime? LessonDate { get; set; }
        public byte? Pair { get; set; }
        public TimeSpan? LessonStart { get; set; }
        public TimeSpan? LessonFinish { get; set; }

        public virtual Curriculum Curriculum { get; set; }
    }
}
