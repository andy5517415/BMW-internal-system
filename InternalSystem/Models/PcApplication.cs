using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcApplication
    {
        public long PurchaseId { get; set; }
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string SupplierId { get; set; }
        public string Comment { get; set; }
        public int Total { get; set; }
        public bool? DeliveryStatus { get; set; }
        public bool? ApplicationStatus { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual PcSupplierList Supplier { get; set; }
    }
}
