using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionProcess
    {
        public ProductionProcess()
        {
            MonitoringProcessAreaStatuses = new HashSet<MonitoringProcessAreaStatus>();
            ProductionProcessLists = new HashSet<ProductionProcessList>();
        }

        public int ProcessId { get; set; }
        public string ProcessName { get; set; }

        public virtual ICollection<MonitoringProcessAreaStatus> MonitoringProcessAreaStatuses { get; set; }
        public virtual ICollection<ProductionProcessList> ProductionProcessLists { get; set; }
    }
}
