using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class Feedback
    {
        [Key]
        public int FeedBackId { get; set; }
        public int CandidateId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InterviewDate { get; set; }
        public bool CandidateStatus { get; set; }
        public string OtherComments { get; set; }
    }
}