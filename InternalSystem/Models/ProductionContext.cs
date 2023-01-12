using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionContext
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public int ProcessId { get; set; }
        public int AreaId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Context { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ProductionProcessList ProductionProcessList { get; set; }
    }
}
