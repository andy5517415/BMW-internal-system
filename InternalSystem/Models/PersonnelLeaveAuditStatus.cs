using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveAuditStatus
    {
        public PersonnelLeaveAuditStatus()
        {
            PersonnelLeaveFormConectStatuses = new HashSet<PersonnelLeaveFormConectStatus>();
        }

        public int StatusId { get; set; }
        public string AuditStatus { get; set; }

        public virtual ICollection<PersonnelLeaveFormConectStatus> PersonnelLeaveFormConectStatuses { get; set; }
    }
}
