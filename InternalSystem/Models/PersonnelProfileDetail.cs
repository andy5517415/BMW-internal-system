using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelProfileDetail
    {
        public PersonnelProfileDetail()
        {
            BusinessOrders = new HashSet<BusinessOrder>();
            MeetingReserves = new HashSet<MeetingReserve>();
            PcApplications = new HashSet<PcApplication>();
            PersonnelAttendanceTimes = new HashSet<PersonnelAttendanceTime>();
            PersonnelLeaveFormEmployees = new HashSet<PersonnelLeaveForm>();
            PersonnelLeaveFormProxyNavigations = new HashSet<PersonnelLeaveForm>();
            PersonnelLeaveOvers = new HashSet<PersonnelLeaveOver>();
            PersonnelOvertimeForms = new HashSet<PersonnelOvertimeForm>();
            ProductionContexts = new HashSet<ProductionContext>();
        }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Sex { get; set; }
        public bool IsMarried { get; set; }
        public string IdentiyId { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string HomePhone { get; set; }
        public string EmergencyPerson { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmergencyNumber { get; set; }
        public string Country { get; set; }
        public int PositionId { get; set; }
        public int RankId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Acount { get; set; }
        public string Password { get; set; }
        public bool DutyStatus { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? Terminationdate { get; set; }
        public string Photo { get; set; }
        public string Note { get; set; }

        public virtual PersonnelCityList City { get; set; }
        public virtual PersonnelDepartmentList Department { get; set; }
        public virtual PersonnelPosition Position { get; set; }
        public virtual PersonnelRank Rank { get; set; }
        public virtual ICollection<BusinessOrder> BusinessOrders { get; set; }
        public virtual ICollection<MeetingReserve> MeetingReserves { get; set; }
        public virtual ICollection<PcApplication> PcApplications { get; set; }
        public virtual ICollection<PersonnelAttendanceTime> PersonnelAttendanceTimes { get; set; }
        public virtual ICollection<PersonnelLeaveForm> PersonnelLeaveFormEmployees { get; set; }
        public virtual ICollection<PersonnelLeaveForm> PersonnelLeaveFormProxyNavigations { get; set; }
        public virtual ICollection<PersonnelLeaveOver> PersonnelLeaveOvers { get; set; }
        public virtual ICollection<PersonnelOvertimeForm> PersonnelOvertimeForms { get; set; }
        public virtual ICollection<ProductionContext> ProductionContexts { get; set; }
    }
}
