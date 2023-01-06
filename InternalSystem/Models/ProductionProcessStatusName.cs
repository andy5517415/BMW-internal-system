using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionProcessStatusName
    {
        public ProductionProcessStatusName()
        {
            ProductionOrderProcessStatuses = new HashSet<ProductionOrderProcessStatus>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<ProductionOrderProcessStatus> ProductionOrderProcessStatuses { get; set; }
    }
}
