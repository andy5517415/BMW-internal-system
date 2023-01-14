using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelOvertimeForm
    {
        public int StartWorkId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public int? TotalTime { get; set; }
        public bool AuditStatus { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
    }
}
