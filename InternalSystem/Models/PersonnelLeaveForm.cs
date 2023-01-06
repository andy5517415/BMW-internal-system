using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveForm
    {
        public PersonnelLeaveForm()
        {
            PersonnelLeaveFormConectStatuses = new HashSet<PersonnelLeaveFormConectStatus>();
        }

        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int StatusId { get; set; }
        public int LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Proxy { get; set; }
        public int AuditManerger { get; set; }
        public int TotalTime { get; set; }
        public string Reason { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual PersonnelLeaveType LeaveTypeNavigation { get; set; }
        public virtual PersonnelProfileDetail ProxyNavigation { get; set; }
        public virtual ICollection<PersonnelLeaveFormConectStatus> PersonnelLeaveFormConectStatuses { get; set; }
    }
}
