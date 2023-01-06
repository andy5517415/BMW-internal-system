using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveType
    {
        public PersonnelLeaveType()
        {
            PersonnelLeaveForms = new HashSet<PersonnelLeaveForm>();
            PersonnelLeaveOvers = new HashSet<PersonnelLeaveOver>();
        }

        public int LeaveTypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<PersonnelLeaveForm> PersonnelLeaveForms { get; set; }
        public virtual ICollection<PersonnelLeaveOver> PersonnelLeaveOvers { get; set; }
    }
}
