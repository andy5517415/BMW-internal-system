using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveFormConectStatus
    {
        public int LeaveId { get; set; }
        public int StatusId { get; set; }
        public DateTime? AuditDate { get; set; }
        public int EmployeeId { get; set; }
        public string AuditOpinion { get; set; }

        public virtual PersonnelLeaveForm Leave { get; set; }
        public virtual PersonnelLeaveAuditStatus Status { get; set; }
    }
}
