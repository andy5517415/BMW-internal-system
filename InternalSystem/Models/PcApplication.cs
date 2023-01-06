using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcApplication
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string SupplierId { get; set; }
        public string Comment { get; set; }
        public int Total { get; set; }

        public virtual PcApplicationRecordSearch Purchase { get; set; }
        public virtual PcSupplierList Supplier { get; set; }
    }
}
