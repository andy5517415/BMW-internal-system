using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelLeaveOver
    {
        public int EmployeeId { get; set; }
        public int LeaveType { get; set; }
        public int Quantity { get; set; }
        public double Used { get; set; }
        public double LeaveOver { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual PersonnelLeaveType LeaveTypeNavigation { get; set; }
    }
}
