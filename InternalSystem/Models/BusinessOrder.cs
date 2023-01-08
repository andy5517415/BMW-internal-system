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
        }

        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDateTime { get; set; }
        public int AreaId { get; set; }
        public int? Price { get; set; }
        public int EmployeeId { get; set; }
        public bool IsAccepted { get; set; }

        public virtual BusinessArea Area { get; set; }
        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ProductionProcessList ProductionProcessList { get; set; }
        public virtual ICollection<BusinessOrderDetail> BusinessOrderDetails { get; set; }
    }
}
