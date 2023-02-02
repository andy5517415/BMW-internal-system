using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcGoodList
    {
        public PcGoodList()
        {
            PcOrderDetails = new HashSet<PcOrderDetail>();
        }

        public int ProductId { get; set; }
        public string Goods { get; set; }
        public string Unit { get; set; }
        public int UnitPrice { get; set; }
        public byte[] Image { get; set; }
        public string Classification { get; set; }

        public virtual ICollection<PcOrderDetail> PcOrderDetails { get; set; }
    }
}
