using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelDepartmentConnectEmployeeId
    {
        public int EmployeeId { get; set; }
        public int DepId { get; set; }

        public virtual PersonnelDepartmentList Dep { get; set; }
    }
}
