using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class BusinessOrderDetail
    {
        public int OdId { get; set; }
        public int OrderId { get; set; }
        public int OptionalId { get; set; }

        public virtual BusinessOptional Optional { get; set; }
        public virtual BusinessOrder Order { get; set; }
    }
}
