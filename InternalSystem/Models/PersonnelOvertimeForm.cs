using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelOvertimeForm
    {
        public int StartWorkId { get; set; }
        public int EmployeeId { get; set; }
        public int PropessId { get; set; }
        public int AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public double TotalTime { get; set; }
        public bool AuditStatus { get; set; }
        public string ApplicationDate { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
    }
}
