using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcApplication
    {
        public PcApplication()
        {
            PcOrderDetails = new HashSet<PcOrderDetail>();
        }

        public long PurchaseId { get; set; }
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int Total { get; set; }
        public bool ApplicationStatus { get; set; }
        public bool ApplicationRejectStatus { get; set; }
        public bool DeliveryStatus { get; set; }
        public bool DeliveryRejectStatus { get; set; }
        public bool AcceptanceStatus { get; set; }
        public bool AcceptanceRejectStatus { get; set; }
        public string RejectReason { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ICollection<PcOrderDetail> PcOrderDetails { get; set; }
    }
}
