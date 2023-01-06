﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelDepartmentList
    {
        public PersonnelDepartmentList()
        {
            PcApplicationRecordSearches = new HashSet<PcApplicationRecordSearch>();
            PersonnelDepartmentConnectEmployeeIds = new HashSet<PersonnelDepartmentConnectEmployeeId>();
        }

        public int DepId { get; set; }
        public string DepName { get; set; }

        public virtual ICollection<PcApplicationRecordSearch> PcApplicationRecordSearches { get; set; }
        public virtual ICollection<PersonnelDepartmentConnectEmployeeId> PersonnelDepartmentConnectEmployeeIds { get; set; }
    }
}
