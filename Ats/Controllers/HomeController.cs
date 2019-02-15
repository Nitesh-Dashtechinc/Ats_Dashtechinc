using Ats.Models;
using Ats.Models.ViewModel;
using Ats.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ats.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        public HomeController()
        {
            db = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult Index()
        {

            List<AtsGridViewModel> list = (from e in db.InterPersonalInfo
                                           join p in db.InterPreEmpDetail on e.CandidateId equals p.CandidateId
                                           select new AtsGridViewModel()
                                           {
                                               CandidateId = e.CandidateId,
                                               FirstName = e.FirstName,
                                               LastName = e.LastName,
                                               CompanyName = p.CompanyName,
                                               Designation = e.AppliedForDesignation,
                                               Department = e.AppliedForDepartment,
                                               WorkFrom = p.WorkFrom,
                                               WorkTo = p.WorkTo,
                                               CtcMonth = p.CtcMonth,
                                               City = e.CityPresent,
                                           }).ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult ViewDetail(int id)
        {

            GridPreInterRegisterViewModel Candidate = new GridPreInterRegisterViewModel();
            InterPersonalInfoViewModel perSonalInfo = new InterPersonalInfoViewModel();
            List<InterPreEmpDetailViewModel> preEmployementDetail = new List<InterPreEmpDetailViewModel>();
            List<InterEducBackgroundViewModel> eduBackGround = new List<InterEducBackgroundViewModel>();
            List<InterReferenceViewModel> reference = new List<InterReferenceViewModel>();
            //List<CityViewModel> cities = new List<CityViewModel>();
            CityViewModel cities = new CityViewModel();

            //List<StateViewModel> states = new List<StateViewModel>();
            StateViewModel states = new StateViewModel();
            DepartmentViewModel departments = new DepartmentViewModel();
            DesignationViewModel designations = new DesignationViewModel();
            GetData getData = new GetData();
            perSonalInfo =
            (
                from p in getData.GetAllPersonalInfo()
                where (p.CandidateId == id)
                select p
            ).FirstOrDefault();
            Candidate.PersonalInfo = perSonalInfo;
            preEmployementDetail = (
                from p in getData.GetAllPreviousEmplpoyementDetails()
                where (p.CandidateId == id)
                select p
                ).ToList();
            Candidate.PreviousEmploymentDetail = preEmployementDetail;

            eduBackGround = (
                 from p in getData.GetAllEducationBackground()
                 where (p.CandidateId == id)
                 select p
                ).ToList();
            Candidate.EducationBackground = eduBackGround;

            reference = (
                 from p in getData.GetReferenceInfo()
                 where (p.CandidateId == id)
                 select p
                ).ToList();
            Candidate.Reference = reference;

            //cities = (
            //    from p in getData.GetAllCity()
            //    where (p.CityId == perSonalInfo.CityPresent && p.CityId == perSonalInfo.CityPast)
            //    select p
            //    ).ToList();
            //Candidate.Cities = cities;
            //states = (
            //        from p in getData.GetAllState()
            //        where (p.StateId == perSonalInfo.StatePresent || p.StateId == perSonalInfo.StatePast )
            //        select p
            //        ).ToList();
            //Candidate.States = states;
            
            //Get StateName Past
            var StateNamePast = (from m in db.InterPersonalInfo
                        //join c in db.State on m.StatePast equals c.StateId
                        where m.CandidateId == id
                        select new
                        {
                           // StateName = c.StateName
                        }).FirstOrDefault();
            Candidate.States = new StateViewModel();
            //Candidate.States.StateNamePast = string.IsNullOrEmpty(StateNamePast.StateName.ToString()) ? "" : StateNamePast.StateName.ToString();
            
            
            //Get StateName Presant
            var StatePresent = (from m in db.InterPersonalInfo
                                 //join c in db.State on m.StatePresent equals c.StateId
                                 where m.CandidateId == id
                                 select new
                                 {
                                    // StateName = c.StateName
                                 }).FirstOrDefault();

            //Candidate.States.StateNamePresant = string.IsNullOrEmpty(StatePresent.StateName.ToString()) ? "" : StatePresent.StateName.ToString();

            //Get CityName Presant
            var CityNamePresant = (from m in db.InterPersonalInfo
                                //join c in db.City on m.CityPresent equals c.CityId 
                                where m.CandidateId == id
                                select new
                                {
                                    //CityName = c.CityName
                                }).FirstOrDefault();
            Candidate.Cities = new CityViewModel();
            //Candidate.Cities.CityNamePresant = string.IsNullOrEmpty(CityNamePresant.CityName.ToString()) ? "" : CityNamePresant.CityName.ToString();

            //Get CityName Past
            var CityNamePast = (from m in db.InterPersonalInfo
                                 //join c in db.City on m.CityPast equals c.CityId
                                 where m.CandidateId == id
                                 select new
                                 {
                                     //CityName = c.CityName
                                 }).FirstOrDefault();
            Candidate.Cities = new CityViewModel();
            //Candidate.Cities.CityNamePast = string.IsNullOrEmpty(CityNamePast.CityName.ToString()) ? "" : CityNamePast.CityName.ToString();

            //Get DepartmentName
            var DepartmentName = (from m in db.InterPersonalInfo
                                //join c in db.Department on m.AppliedForDepartment equals c.DepartmentId
                                where m.CandidateId == id
                                select new
                                {
                                    //Department = c.DepartmentName
                                }).FirstOrDefault();
            Candidate.Departments = new DepartmentViewModel();
           // Candidate.Departments.DepartmentName = string.IsNullOrEmpty(DepartmentName.Department.ToString()) ? "" : DepartmentName.Department.ToString();

            //Get DesignationName
            var DesignationName = (from m in db.InterPersonalInfo
                                  //join c in db.Designation on m.AppliedForDesignation equals c.DesignationId
                                  where m.CandidateId == id
                                  select new
                                  {
                                      //Designation = c.DesignationName
                                  }).FirstOrDefault();
            //Candidate.Designation = new DesignationViewModel();
            //Candidate.Designation.DesignationName = string.IsNullOrEmpty(DesignationName.Designation.ToString()) ? "" : DesignationName.Designation.ToString();

            //departments = (
            //         from p in getData.GetAllDepartment()
            //         where (p.DepartmentId == perSonalInfo.AppliedForDepartment)
            //         select p
            //         ).ToList();
            //Candidate.Departments = departments;

            //designations = (
            //         from p in getData.GetAllDesignation()
            //         where (p.DesignationId == perSonalInfo.AppliedForDesignation)
            //         select p
            //         ).ToList();
            //Candidate.Designation = designations;

            return View(Candidate);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            List<SelectListItem> getStateList = (from p in db.State.AsEnumerable()
                                                 select new SelectListItem
                                                 {
                                                     Text = p.StateName,
                                                     Value = p.StateId.ToString()
                                                 }).ToList();
            getStateList.Insert(0, new SelectListItem { Text = "--Select State--", Value = "" });
            ViewBag.stateList = getStateList;
            List<SelectListItem> getDepartmentList = (from p in db.Department.AsEnumerable()
                                                      select new SelectListItem
                                                      {
                                                          Text = p.DepartmentName,
                                                          Value = p.DepartmentId.ToString()
                                                      }).ToList();
            getDepartmentList.Insert(0, new SelectListItem { Text = "--Select Department--", Value = "" });
            ViewBag.departmentList = getDepartmentList;
            return View();
        }
       
        //home/GetCity
        [HttpGet]
        [Route("GetCity")]
        public JsonResult GetCity(int id)
        {

            JsonResult result = new JsonResult();
            result.Data = db.City.Select(s => new { s.CityId, s.CityName, s.StateId }).Where(city => city.StateId == id).OrderBy(city => city.CityName).ToList();
            result.ContentType = "Succeess";
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }
        //home/GetDesignation
        [HttpGet]
        [Route("GetDesignation")]
        public JsonResult GetDesignation(int id)
        {
            try
            {
                JsonResult result = new JsonResult();
                result.Data = db.Designation.Where(designation => designation.DepartmentId == id).Select(ds => new { ds.DesignationId, ds.DesignationName }).ToList();
                result.ContentType = "Succeess";
                return Json(result.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SavePreInterView(PreInterRegisterViewModel obj)
        {
            JsonResult res = new JsonResult();

            try
            {
                DateTime now = DateTime.Now;
                obj.PersonalInfo.CreatedDate = now;
                db.InterPersonalInfo.Add(obj.PersonalInfo);
                db.SaveChanges();
                var id = obj.PersonalInfo.CandidateId;
                if (id != 0)
                {
                    foreach (InterPreEmpDetail a in obj.PreviousEmploymentDetail)
                    {
                        a.CandidateId = id;
                        db.InterPreEmpDetail.Add(a);
                        db.SaveChanges();
                    }
                    foreach (InterReference b in obj.Reference)
                    {
                        b.CandidateId = id;
                        db.InterReference.Add(b);
                        db.SaveChanges();
                    }
                    foreach (InterEducBackground c in obj.EducationBackground)
                    {
                        c.CandidateId = id;
                        db.InterEducBackground.Add(c);
                        db.SaveChanges();
                    }
                }
                res.ContentType = "success";
                res.Data = "Your Form Submited Successfully";
            }
            catch (Exception ex)
            {

                res.ContentType = "error";
                res.Data = string.IsNullOrEmpty(ex.InnerException.ToString()) ? ex.Message.ToString() : ex.InnerException.ToString();
            }

            return Json(res);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}