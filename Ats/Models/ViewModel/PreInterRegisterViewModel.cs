using Ats.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class PreInterRegisterViewModel
    {
        public InterPersonalInfo PersonalInfo { get; set; }
        public List<InterPreEmpDetail> PreviousEmploymentDetail { get; set; }
        public List<InterReference> Reference { get; set; }
        public List<InterEducBackground> EducationBackground { get; set; }
        public List<InterLanguage> Languages { get; set; }
        
    }
}