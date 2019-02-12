using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class InterReferenceViewModel
    {
        public int ReferenceId { get; set; }
        public int CandidateId { get; set; }
        public string PersonName { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
    }
}