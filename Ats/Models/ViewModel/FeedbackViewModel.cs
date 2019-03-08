using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class FeedbackViewModel
    {
        public int CandidateId { get; set; }
        public string InterviewDate { get; set; }
        public bool CandidateStatus { get; set; }
        public string OtherComments { get; set; }
    }
}