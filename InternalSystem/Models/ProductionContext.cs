using System;
using System.Collections.Generic;
using System.Data.SqlTypes;



#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionContext
    {
        public int OrderId { get; set; }
        public int ProcessId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Context { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ProductionProcessList Order { get; set; }
        public virtual ProductionProcess Process { get; set; }
    }
}
