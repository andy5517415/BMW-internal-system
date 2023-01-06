using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionOrderProcessStatus
    {
        public int OrderId { get; set; }
        public int ProcessId { get; set; }
        public int StatusId { get; set; }

        public virtual ProductionProcessList Order { get; set; }
        public virtual ProductionProcess Process { get; set; }
        public virtual ProductionProcessStatusName Status { get; set; }
    }
}
