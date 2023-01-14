using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PcApplicationRecordSearch
    {
        public int PurchaseId { get; set; }
        public int DepId { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public int Total { get; set; }
        public bool DeliveryStatus { get; set; }

        public virtual PcApplication PcApplication { get; set; }
    }
}
