using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MonitoringStatus
    {
        public MonitoringStatus()
        {
            MonitoringProcessAreaStatuses = new HashSet<MonitoringProcessAreaStatus>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<MonitoringProcessAreaStatus> MonitoringProcessAreaStatuses { get; set; }
    }
}
