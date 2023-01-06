using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class ProductionBugContext
    {
        public int BugContextId { get; set; }
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }

        public virtual ProductionProcessList Order { get; set; }
    }
}
