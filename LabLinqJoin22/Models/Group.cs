using System;
using System.Collections.Generic;

#nullable disable

namespace LabLinqJoin22.Models
{
    public partial class Group
    {
        public Group()
        {
            Curricula = new HashSet<Curriculum>();
            Students = new HashSet<Student>();
        }

        public int GroupId { get; set; }
        public string GroupNumber { get; set; }
        public int? ChairId { get; set; }
        public int? CuratorId { get; set; }
        public int? SeniorStudentId { get; set; }
        public int? StudyHours { get; set; }
        public int? LabStudies { get; set; }
        public int? PractStudies { get; set; }

        public virtual Chair Chair { get; set; }
        public virtual Tutor Curator { get; set; }
        public virtual Student SeniorStudent { get; set; }
        public virtual ICollection<Curriculum> Curricula { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
