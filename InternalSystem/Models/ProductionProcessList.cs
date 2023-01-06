﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionProcessList
    {
        public ProductionProcessList()
        {
            ProductionBugContexts = new HashSet<ProductionBugContext>();
            ProductionContexts = new HashSet<ProductionContext>();
            ProductionOrderProcessStatuses = new HashSet<ProductionOrderProcessStatus>();
        }

        public int OrderId { get; set; }
        public int? AreaId { get; set; }
        public DateTime? StarDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ProductionArea Area { get; set; }
        public virtual BusinessOrder Order { get; set; }
        public virtual ICollection<ProductionBugContext> ProductionBugContexts { get; set; }
        public virtual ICollection<ProductionContext> ProductionContexts { get; set; }
        public virtual ICollection<ProductionOrderProcessStatus> ProductionOrderProcessStatuses { get; set; }
    }
}
