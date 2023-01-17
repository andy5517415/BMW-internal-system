using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class BusinessOptional
    {
        public BusinessOptional()
        {
            BusinessOrderDetails = new HashSet<BusinessOrderDetail>();
        }

        public int OptionalId { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public string OptionalName { get; set; }
        public string Photo { get; set; }

        public virtual BusinessCategory Category { get; set; }
        public virtual ICollection<BusinessOrderDetail> BusinessOrderDetails { get; set; }
    }
}
