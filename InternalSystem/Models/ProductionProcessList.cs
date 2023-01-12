using System;
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
        }

        public int OrderId { get; set; }
        public int ProcessId { get; set; }
        public int? AreaId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }

        public virtual ProductionArea Area { get; set; }
        public virtual BusinessOrder Order { get; set; }
        public virtual ProductionProcess Process { get; set; }
        public virtual ProductionProcessStatusName Status { get; set; }
        public virtual ICollection<ProductionBugContext> ProductionBugContexts { get; set; }
        public virtual ICollection<ProductionContext> ProductionContexts { get; set; }
    }
}
