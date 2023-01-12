using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionProcessStatusName
    {
        public ProductionProcessStatusName()
        {
            ProductionProcessLists = new HashSet<ProductionProcessList>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<ProductionProcessList> ProductionProcessLists { get; set; }
    }
}
