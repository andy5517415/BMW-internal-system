using System;

namespace InternalSystem.Dotos
{
    public class Leftjoin
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? EditDatetime { get; set; }
        public bool IsAccepted { get; set; }
        //public string OptionalName { get; set; }
        public string AreaName { get; set; }
        public string AreaNameProcess { get; set; }
        public string ProcessName { get; set; }
        //public int StatusId { get; set; }
    }
}
