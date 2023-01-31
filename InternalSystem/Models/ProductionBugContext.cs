using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionBugContext
    {
        public int OrderId { get; set; }
        public int ProcessId { get; set; }
        public int AreaId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Rank { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Dispose { get; set; }
        public string Photo { get; set; }

        public virtual ProductionProcessList ProductionProcessList { get; set; }
    }
}
