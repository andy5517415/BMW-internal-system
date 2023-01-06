using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelRank
    {
        public PersonnelRank()
        {
            PersonnelProfileDetails = new HashSet<PersonnelProfileDetail>();
        }

        public int RankId { get; set; }
        public string Rank { get; set; }

        public virtual ICollection<PersonnelProfileDetail> PersonnelProfileDetails { get; set; }
    }
}
