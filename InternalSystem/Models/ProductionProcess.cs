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
            ProductionContexts = new HashSet<ProductionContext>();
            ProductionOrderProcessStatuses = new HashSet<ProductionOrderProcessStatus>();
        }

        public int ProcessId { get; set; }
        public string ProcessName { get; set; }

        public virtual ICollection<MonitoringProcessAreaStatus> MonitoringProcessAreaStatuses { get; set; }
        public virtual ICollection<ProductionContext> ProductionContexts { get; set; }
        public virtual ICollection<ProductionOrderProcessStatus> ProductionOrderProcessStatuses { get; set; }
    }
}
