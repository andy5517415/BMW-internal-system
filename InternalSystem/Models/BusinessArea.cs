using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class BusinessArea
    {
        public BusinessArea()
        {
            BusinessOrders = new HashSet<BusinessOrder>();
        }

        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<BusinessOrder> BusinessOrders { get; set; }
    }
}
