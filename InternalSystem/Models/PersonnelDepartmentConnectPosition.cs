using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelDepartmentConnectPosition
    {
        public int DepId { get; set; }
        public int PositonId { get; set; }
        public string PositonName { get; set; }

        public virtual PersonnelDepartmentList Dep { get; set; }
    }
}
