using Ats.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class GridPreInterRegisterViewModel
    {
        public InterPersonalInfoViewModel PersonalInfo { get; set; }
        public List<InterPreEmpDetailViewModel> PreviousEmploymentDetail { get; set; }
        public List<InterReferenceViewModel> Reference { get; set; }
        public List<InterEducBackgroundViewModel> EducationBackground { get; set; }
        //public List<CityViewModel> Cities { get; set; }
        public CityViewModel Cities { get; set; }
        //public List<StateViewModel> States { get; set; }
        public StateViewModel States { get; set; }
        //public List<DesignationViewModel> Designation { get; set; }
        public DepartmentViewModel Departments { get; set; }
        public DesignationViewModel Designation { get; set; }
        public List<LanguageViewModel> Language { get; set; }
        //public List<DepartmentViewModel> Departments { get; set; }
    }
}