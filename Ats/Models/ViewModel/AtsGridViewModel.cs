using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class AtsGridViewModel
    {
        public int CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EarliestJoinDate { get; set; }
        public string TotalExperienceInYear { get; set; }
        public string CityPresent { get; set; }
        public string SalaryExpectation { get; set; }
    }
}