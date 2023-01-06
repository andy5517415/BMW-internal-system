using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcSupplierList
    {
        public PcSupplierList()
        {
            PcApplications = new HashSet<PcApplication>();
            PcPurchaseItemSearches = new HashSet<PcPurchaseItemSearch>();
        }

        public string SupplierId { get; set; }
        public string SupplierContact { get; set; }
        public string SupplierContactPerson { get; set; }
        public string SupplierPhone { get; set; }

        public virtual ICollection<PcApplication> PcApplications { get; set; }
        public virtual ICollection<PcPurchaseItemSearch> PcPurchaseItemSearches { get; set; }
    }
}
