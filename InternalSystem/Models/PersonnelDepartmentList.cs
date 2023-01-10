﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelDepartmentList
    {
        public PersonnelDepartmentList()
        {
            PersonnelDepartmentConnectEmployeeIds = new HashSet<PersonnelDepartmentConnectEmployeeId>();
            PersonnelProfileDetails = new HashSet<PersonnelProfileDetail>();
        }

        public int DepartmentId { get; set; }
        public string DepName { get; set; }

        public virtual ICollection<PersonnelDepartmentConnectEmployeeId> PersonnelDepartmentConnectEmployeeIds { get; set; }
        public virtual ICollection<PersonnelProfileDetail> PersonnelProfileDetails { get; set; }
    }
}
