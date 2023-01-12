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

>>>>>>> 2ac6ade0fe0d4d294f96a489921b1fcc34a7d002
        public int ProcessId { get; set; }
        public int AreaId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
<<<<<<< HEAD
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
=======


>>>>>>> 2ac6ade0fe0d4d294f96a489921b1fcc34a7d002
        public string Context { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual ProductionProcessList ProductionProcessList { get; set; }
    }
}
