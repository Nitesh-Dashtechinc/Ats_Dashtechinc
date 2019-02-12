using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class InterEducBackgroundViewModel
    {
        public int EducationalId { get; set; }
        public int CandidateId { get; set; }
        public string BoardUniversityName { get; set; }
        public string CourseDegreeName { get; set; }
        public string PassingYear { get; set; }
        public string GradePercentage { get; set; }

    }
}