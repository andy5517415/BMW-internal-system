using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionArea
    {
        public ProductionArea()
        {
            MonitoringProcessAreaStatuses = new HashSet<MonitoringProcessAreaStatus>();
            ProductionProcessLists = new HashSet<ProductionProcessList>();
        }

        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public virtual ICollection<MonitoringProcessAreaStatus> MonitoringProcessAreaStatuses { get; set; }
        public virtual ICollection<ProductionProcessList> ProductionProcessLists { get; set; }
    }
}
