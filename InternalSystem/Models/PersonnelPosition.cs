using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelPosition
    {
        public PersonnelPosition()
        {
            PersonnelProfileDetails = new HashSet<PersonnelProfileDetail>();
        }

        public int PositionId { get; set; }
        public string PositionName { get; set; }

        public virtual ICollection<PersonnelProfileDetail> PersonnelProfileDetails { get; set; }
    }
}
