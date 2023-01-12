using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelAttendanceTime
    {
        public int AttendId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string AttendTime { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
    }
}
