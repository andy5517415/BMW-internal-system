using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MonitoringProcessAreaStatus
    {
        public int AreaId { get; set; }
        public int ProcessId { get; set; }
        public string StatusId { get; set; }
        public string CarType { get; set; }

        public virtual ProductionArea Area { get; set; }
        public virtual ProductionProcess Process { get; set; }
    }
}
