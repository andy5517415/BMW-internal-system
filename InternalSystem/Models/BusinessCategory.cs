using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class BusinessCategory
    {
        public BusinessCategory()
        {
            BusinessOptionals = new HashSet<BusinessOptional>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<BusinessOptional> BusinessOptionals { get; set; }
    }
}
