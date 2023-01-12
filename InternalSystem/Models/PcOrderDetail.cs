using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcOrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Goods { get; set; }
        public int Quantiy { get; set; }
        public string Unit { get; set; }
        public int UnitPrice { get; set; }
        public int Subtotal { get; set; }

        public virtual PcPurchaseItemSearch Product { get; set; }
    }
}
