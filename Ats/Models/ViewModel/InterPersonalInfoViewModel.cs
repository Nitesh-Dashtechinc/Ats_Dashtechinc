using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModels
{
    public class InterPersonalInfoViewModel
    {
        public int CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string MaritalStaus { get; set; }
        public int? NoOfChildren { get; set; }
        public string AddressPresent { get; set; }
        public string StatePresent { get; set; }
        public string CityPresent { get; set; }
        public string PincodePresent { get; set; }
        public string AddressPast { get; set; }
        public string StatePast { get; set; }
        public string CityPast { get; set; }
        public string PinCodePast { get; set; }
        public string AppliedForDepartment { get; set; }
        public string AppliedForDesignation { get; set; }
        public string TotalExperienceInYear { get; set; }
        public string EarliestJoinDate { get; set; }
        public string SalaryExpectation { get; set; }
        public string Vehicle { get; set; }
        public string JobSource { get; set; }

        public bool NightShift { get; set; }

        public bool IsReference { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceMobileNo { get; set; }
        public string ReferenceDesignation { get; set; }

        public string EmailId { get; set; }
        public string OtherCertification { get; set; }
        public string OtherComments { get; set; }
    }
}