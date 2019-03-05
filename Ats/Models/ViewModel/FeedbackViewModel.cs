using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class FeedbackViewModel
    {
        public int CandidateId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string CandidateStatus { get; set; }
        public string OtherComments { get; set; }
    }
}