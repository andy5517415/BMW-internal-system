using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MeetingRoom
    {
        public MeetingRoom()
        {
            MeetingRecords = new HashSet<MeetingRecord>();
        }

        public int MeetingPlaceId { get; set; }
        public string MeetingRoom1 { get; set; }

        public virtual ICollection<MeetingRecord> MeetingRecords { get; set; }
    }
}
