using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MonitoringProcessAreaStatus
    {
        public int AreaId { get; set; }
        public int ProcessId { get; set; }
        public int StatusId { get; set; }

        public virtual ProductionArea Area { get; set; }
        public virtual ProductionProcess Process { get; set; }
        public virtual MonitoringStatus Status { get; set; }
    }
}
