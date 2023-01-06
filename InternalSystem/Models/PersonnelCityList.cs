using System;
using System.Collections.Generic;

#nullable disable

namespace InternalSystem.Models
{
    public partial class PersonnelCityList
    {
        public PersonnelCityList()
        {
            PersonnelProfileDetails = new HashSet<PersonnelProfileDetail>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<PersonnelProfileDetail> PersonnelProfileDetails { get; set; }
    }
}
