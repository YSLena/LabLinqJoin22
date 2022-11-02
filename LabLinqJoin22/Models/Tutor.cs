using System;
using System.Collections.Generic;

#nullable disable

namespace LabLinqJoin22.Models
{
    public partial class Tutor
    {
        public Tutor()
        {
            ChairChairHeads = new HashSet<Chair>();
            ChairDeputyDeans = new HashSet<Chair>();
            Curricula = new HashSet<Curriculum>();
            Groups = new HashSet<Group>();
        }

        public int TutorId { get; set; }
        public string NameFio { get; set; }
        public int? Faculty { get; set; }
        public int? ChairId { get; set; }
        public string ChairExternal { get; set; }
        public string Position { get; set; }

        public virtual Chair Chair { get; set; }
        public virtual ICollection<Chair> ChairChairHeads { get; set; }
        public virtual ICollection<Chair> ChairDeputyDeans { get; set; }
        public virtual ICollection<Curriculum> Curricula { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
