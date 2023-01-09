using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelDepartmentList
    {
        public PersonnelDepartmentList()
        {
            PersonnelDepartmentConnectEmployeeIds = new HashSet<PersonnelDepartmentConnectEmployeeId>();
        }

        public int DepId { get; set; }
        public string DepName { get; set; }

        public virtual ICollection<PersonnelDepartmentConnectEmployeeId> PersonnelDepartmentConnectEmployeeIds { get; set; }
    }
}
