using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveAuditStatus
    {
        public PersonnelLeaveAuditStatus()
        {
            PersonnelLeaveForms = new HashSet<PersonnelLeaveForm>();
        }

        public int StatusId { get; set; }
        public string AuditStatus { get; set; }

        public virtual ICollection<PersonnelLeaveForm> PersonnelLeaveForms { get; set; }
    }
}
