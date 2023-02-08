using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class BusinessOrder
    {
        public BusinessOrder()
        {
            BusinessOrderDetails = new HashSet<BusinessOrderDetail>();
            ProductionProcessLists = new HashSet<ProductionProcessList>();
        }

        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? EditDatetime { get; set; }
        public DateTime? DeadlineDateTime { get; set; }
        public int AreaId { get; set; }
        public int? Price { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsAccepted { get; set; }

        public virtual BusinessArea Area { get; set; }
        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ICollection<BusinessOrderDetail> BusinessOrderDetails { get; set; }
        public virtual ICollection<ProductionProcessList> ProductionProcessLists { get; set; }
    }
}
