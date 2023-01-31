using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MeetingRecord
    {
        public int RecordSheetId { get; set; }
        public int BookMeetId { get; set; }
        public string MeetPresident { get; set; }
        public string Rcorder { get; set; }
        public string Participater { get; set; }
        public int ShouldAttend { get; set; }
        public int Attend { get; set; }
        public int? NoAttend { get; set; }
        public string NoAttendPerson { get; set; }
        public DateTime Date { get; set; }
        public string Agenda { get; set; }
        public string Record { get; set; }

        public virtual MeetingReserve BookMeet { get; set; }
    }
}
