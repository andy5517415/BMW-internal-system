using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionContext
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> b8d7905c5a839ce3cde8f9363330c01a04bd10c1
        public int ProcessId { get; set; }
        public int AreaId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
<<<<<<< HEAD
=======
=======
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
>>>>>>> 50a12a42f8d98e1d0587ea8864531cbd0abc3e5b
>>>>>>> b8d7905c5a839ce3cde8f9363330c01a04bd10c1
        public string Context { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ProductionProcessList ProductionProcessList { get; set; }
    }
}
