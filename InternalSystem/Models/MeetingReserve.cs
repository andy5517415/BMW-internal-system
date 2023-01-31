using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MeetingReserve
    {
        public MeetingReserve()
        {
            MeetingRecords = new HashSet<MeetingRecord>();
        }

        public int BookMeetId { get; set; }
        public int EmployeeId { get; set; }
        public int MeetPlaceId { get; set; }
        public string MeetType { get; set; }
        public int DepId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public virtual PersonnelProfileDetail Employee { get; set; }
        public virtual MeetingRoom MeetPlace { get; set; }
        public virtual ICollection<MeetingRecord> MeetingRecords { get; set; }
    }
}
