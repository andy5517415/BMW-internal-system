using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveForm
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string ApplicationDate { get; set; }
        public int StatusId { get; set; }
        public int LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public int? Proxy { get; set; }
        public int? AuditManerger { get; set; }
        public double? TotalTime { get; set; }
        public string Reason { get; set; }
        public bool? ProxyAudit { get; set; }
        public DateTime? ProxyAuditDate { get; set; }
        public bool? ManagerAudit { get; set; }
        public DateTime? ManagerAuditDate { get; set; }
        public string AuditOpnion { get; set; }
        public string Photo { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual PersonnelLeaveType LeaveTypeNavigation { get; set; }
        public virtual PersonnelProfileDetail ProxyNavigation { get; set; }
        public virtual PersonnelLeaveAuditStatus Status { get; set; }
    }
}
