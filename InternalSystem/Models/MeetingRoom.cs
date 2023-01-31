using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MeetingRoom
    {
        public MeetingRoom()
        {
            MeetingReserves = new HashSet<MeetingReserve>();
        }

        public int MeetingPlaceId { get; set; }
        public string MeetingRoom1 { get; set; }

        public virtual ICollection<MeetingReserve> MeetingReserves { get; set; }
    }
}
