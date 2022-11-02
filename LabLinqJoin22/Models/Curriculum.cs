using System;
using System.Collections.Generic;

#nullable disable

namespace LabLinqJoin22.Models
{
    public partial class Curriculum
    {
        public Curriculum()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int CurriculumId { get; set; }
        public int? SubjectId { get; set; }
        public int? TutorId { get; set; }
        public int? GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Tutor Tutor { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
