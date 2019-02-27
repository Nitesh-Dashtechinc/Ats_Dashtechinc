using Ats.Models;
using Ats.Models.ViewModel;
using Ats.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
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
            JsonResult res = new JsonResult();
            try
            {
                List<AtsGridViewModel> list = (from e in db.InterPersonalInfo
                                               join p in db.InterPreEmpDetail on e.CandidateId equals p.CandidateId into ps
                                               from p in ps.DefaultIfEmpty()
                                               select new AtsGridViewModel()
                                               {
                                                   CandidateId = e.CandidateId,
                                                   FirstName = e.FirstName,
                                                   LastName = e.LastName,
                                                   EmailId = e.EmailId,
                                                   Designation = e.AppliedForDesignation,
                                                   Department = e.AppliedForDepartment,
                                                   EarliestJoinDate = e.EarliestJoinDate,
                                                   SalaryExpectation = e.SalaryExpectation,
                                                   TotalExperienceInYear = e.TotalExperienceInYear,
                                                   CityPresent = e.CityPresent
                                                   //CompanyName = p == null ? "-" : p.CompanyName,
                                                   //Department = e.AppliedForDepartment,
                                                   //WorkFrom = p == null ? "-" : p.WorkFrom,
                                                   //WorkTo = string.IsNullOrEmpty(p.WorkTo) ? "-" : p.WorkTo,
                                               }).GroupBy(x => x.CandidateId).Select(x => x.FirstOrDefault()).OrderByDescending(a => a.CandidateId).ToList();


                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                return View();
            }

        }
        public ActionResult Register()
        {
            JsonResult res = new JsonResult();
            try
            {

                TempData.Remove("candidateId"); // Remove Particular TempData i.e. candidateId.
                TempData.Clear();
                List<SelectListItem> getStateList = (from p in db.State.AsEnumerable()
                                                     select new SelectListItem
                                                     {
                                                         Text = p.StateName,
                                                         Value = p.StateId.ToString()
                                                     }).ToList();
                getStateList.Insert(0, new SelectListItem { Text = "--Select State--", Value = "" });
                ViewBag.stateList = getStateList;
                List<SelectListItem> getPermanentStateList = (from p in db.State.AsEnumerable()
                                                              select new SelectListItem
                                                              {
                                                                  Text = p.StateName,
                                                                  Value = p.StateId.ToString()
                                                              }).ToList();
                getPermanentStateList.Insert(0, new SelectListItem { Text = "--Select State--", Value = "" });
                ViewBag.PermanentStateList = getPermanentStateList;
                List<SelectListItem> getDepartmentList = (from p in db.Department.AsEnumerable()
                                                          select new SelectListItem
                                                          {
                                                              Text = p.DepartmentName,
                                                              Value = p.DepartmentId.ToString()
                                                          }).ToList();
                getDepartmentList.Insert(0, new SelectListItem { Text = "--Select Department--", Value = "" });
                ViewBag.departmentList = getDepartmentList;

                List<SelectListItem> getLanguage = (from p in db.InterLanguages.AsEnumerable()
                                                    select new SelectListItem
                                                    {
                                                        Text = p.LanguageType,
                                                        Value = p.LanguageId.ToString()
                                                    }).ToList();
                getLanguage.Insert(0, new SelectListItem { Text = "--Select Language--", Value = "" });
                ViewBag.language = getLanguage;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                return View();
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
                    if (obj.PreviousEmploymentDetail != null)
                    {
                        foreach (InterPreEmpDetail a in obj.PreviousEmploymentDetail)
                        {
                            a.CandidateId = id;
                            db.InterPreEmpDetail.Add(a);
                            db.SaveChanges();
                        }
                    }
                    if (obj.Reference != null)
                    {
                        foreach (InterReference b in obj.Reference)
                        {
                            b.CandidateId = id;
                            db.InterReference.Add(b);
                            db.SaveChanges();
                        }
                    }
                    if (obj.EducationBackground != null)
                    {
                        foreach (InterEducBackground c in obj.EducationBackground)
                        {
                            c.CandidateId = id;
                            db.InterEducBackground.Add(c);
                            db.SaveChanges();
                        }
                    }
                    if (obj.Languages != null)
                    {
                        foreach (InterLanguage l in obj.Languages)
                        {
                            l.CandidateId = id;
                            db.InterLanguages.Add(l);
                            db.SaveChanges();
                        }
                    }
                    res.ContentType = "success";
                    res.Data = "Your Form Submited Successfully";
                    TempData["candidateId"] = id;
                }

            }
            catch (Exception ex)
            {
                res.ContentType = "error";
                res.Data = "Some thing went rong please try agnain";
                return Json(res);
            }

            return Json(res);
        }

        [Authorize]
        public ActionResult ViewDetail(int id)
        {
            JsonResult res = new JsonResult();
            try
            {
                GridPreInterRegisterViewModel Candidate = new GridPreInterRegisterViewModel();
                InterPersonalInfoViewModel perSonalInfo = new InterPersonalInfoViewModel();
                List<InterPreEmpDetailViewModel> preEmployementDetail = new List<InterPreEmpDetailViewModel>();
                List<InterEducBackgroundViewModel> eduBackGround = new List<InterEducBackgroundViewModel>();
                List<InterReferenceViewModel> reference = new List<InterReferenceViewModel>();
                CityViewModel cities = new CityViewModel();
                StateViewModel states = new StateViewModel();
                DepartmentViewModel departments = new DepartmentViewModel();
                DesignationViewModel designations = new DesignationViewModel();
                perSonalInfo =                (                    from p in db.InterPersonalInfo                    where (p.CandidateId == id)                    select new InterPersonalInfoViewModel()
                    {                        CandidateId = p.CandidateId,                        FirstName = p.FirstName,                        LastName = p.LastName,                        MobileNo1 = p.MobileNo1,                        MobileNo2 = p.MobileNo2,                        DateOfBirth = p.DateOfBirth,                        Age = p.Age,                        Gender = p.Gender,                        MaritalStaus = p.Gender,                        NoOfChildren = p.NoOfChildren,                        AddressPresent = p.AddressPresent,                        StatePresent = p.StatePresent,                        CityPresent = p.CityPresent,                        PincodePresent = p.PincodePresent,                        AddressPast = p.AddressPast,                        StatePast = p.StatePast,                        CityPast = p.CityPast,                        PinCodePast = p.PinCodePast,                        AppliedForDepartment = p.AppliedForDepartment,                        AppliedForDesignation = p.AppliedForDesignation,                        TotalExperienceInYear = p.TotalExperienceInYear,                        EarliestJoinDate = p.EarliestJoinDate,                        SalaryExpectation = p.SalaryExpectation,                        Vehicle = p.Vehicle,                        JobSource = p.JobSource,                        NightShift = p.NightShift,                        IsReference = p.IsReference,                        ReferenceName = p.ReferenceName,                        ReferenceDesignation = p.ReferenceDesignation,                        ReferenceMobileNo = p.ReferenceMobileNo,                        EmailId = p.EmailId,                        OtherCertification = p.OtherCertification,                        OtherComments=p.OtherComments                    }).FirstOrDefault();

                Candidate.PersonalInfo = perSonalInfo;
                preEmployementDetail = (from p in db.InterPreEmpDetail
                                        where (p.CandidateId == id)
                                        select new InterPreEmpDetailViewModel
                                        {
                                            EmploymentId = p.EmploymentId,
                                            CandidateId = p.CandidateId,
                                            City = p.City,
                                            CompanyName = p.CompanyName,
                                            Designation = p.Designation,
                                            WorkFrom = p.WorkFrom,
                                            WorkTo = p.WorkTo,
                                            CtcMonth = p.CtcMonth
                                        }).ToList();

                Candidate.PreviousEmploymentDetail = preEmployementDetail;

                eduBackGround = (from p in db.InterEducBackground
                                 where (p.CandidateId == id)
                                 select new InterEducBackgroundViewModel()
                                 {
                                     EducationalId = p.EducationalId,
                                     CandidateId = p.CandidateId,
                                     BoardUniversityName = p.BoardUniversityName,
                                     CourseDegreeName = p.CourseDegreeName,
                                     PassingYear = p.PassingYear,
                                     GradePercentage = p.GradePercentage
                                 }).ToList();
                Candidate.EducationBackground = eduBackGround;

                reference = (from p in db.InterReference
                             where (p.CandidateId == id)
                             select new InterReferenceViewModel()
                             {
                                 ReferenceId = p.ReferenceId,
                                 CandidateId = p.CandidateId,
                                 PersonName = p.PersonName,
                                 CompanyName = p.CompanyName,
                                 Designation = p.Designation,
                                 ContactNo = p.ContactNo
                             }).ToList();
                Candidate.Reference = reference;
                Candidate.Language = new List<LanguageViewModel>();
                Candidate.Language = (from l in db.InterLanguages
                                      where l.CandidateId == id
                                      select new LanguageViewModel
                                      {
                                          LanguageType = l.LanguageType,
                                          Read = l.Read,
                                          Write = l.Write,
                                          Speak = l.Speak
                                      }).ToList();
                return View(Candidate);
            }
            catch (Exception ex)
            {
                ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                return View();
            }
        }

        //home/GetCity
        [HttpGet]
        [Route("GetCity")]
        public JsonResult GetCity(int id)
        {
            JsonResult res = new JsonResult();
            try
            {
                res.Data = db.City.Select(s => new { s.CityId, s.CityName, s.StateId }).Where(city => city.StateId == id).OrderBy(city => city.CityName).ToList();
                res.ContentType = "Succeess";
                return Json(res.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                res.Data = "Unable to get citylist";
                return Json(res.Data, JsonRequestBehavior.AllowGet);
                //ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                //return View();
            }


        }
        //home/GetDesignation
        [HttpGet]
        [Route("GetDesignation")]
        public JsonResult GetDesignation(int id)
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = db.Designation.Where(designation => designation.DepartmentId == id).Select(ds => new { ds.DesignationId, ds.DesignationName }).ToList();
                result.ContentType = "Succeess";
                return Json(result.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
                return Json(result.Data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PreviewDetail()
        {
            JsonResult res = new JsonResult();
            try
            {

                if (TempData.ContainsKey("candidateId"))
                {
                    int id = int.Parse(TempData["candidateId"].ToString());
                    GridPreInterRegisterViewModel Candidate = new GridPreInterRegisterViewModel();
                    InterPersonalInfoViewModel perSonalInfo = new InterPersonalInfoViewModel();
                    List<InterPreEmpDetailViewModel> preEmployementDetail = new List<InterPreEmpDetailViewModel>();
                    List<InterEducBackgroundViewModel> eduBackGround = new List<InterEducBackgroundViewModel>();
                    List<InterReferenceViewModel> reference = new List<InterReferenceViewModel>();
                    CityViewModel cities = new CityViewModel();
                    StateViewModel states = new StateViewModel();
                    DepartmentViewModel departments = new DepartmentViewModel();
                    DesignationViewModel designations = new DesignationViewModel();
                    //Get PersonalInformation of candidate
                    perSonalInfo =(from p in db.InterPersonalInfo
                                    where (p.CandidateId == id)
                                    select new InterPersonalInfoViewModel()
                                    {
                                        CandidateId = p.CandidateId,
                                        FirstName = p.FirstName,
                                        LastName = p.LastName,
                                        MobileNo1 = p.MobileNo1,
                                        MobileNo2 = p.MobileNo2,
                                        DateOfBirth = p.DateOfBirth,
                                        Age = p.Age,
                                        Gender = p.Gender,
                                        MaritalStaus = p.Gender,
                                        NoOfChildren = p.NoOfChildren,
                                        AddressPresent = p.AddressPresent,
                                        StatePresent = p.StatePresent,
                                        CityPresent = p.CityPresent,
                                        PincodePresent = p.PincodePresent,
                                        AddressPast = p.AddressPast,
                                        StatePast = p.StatePast,
                                        CityPast = p.CityPast,
                                        PinCodePast = p.PinCodePast,
                                        AppliedForDepartment = p.AppliedForDepartment,
                                        AppliedForDesignation = p.AppliedForDesignation,
                                        TotalExperienceInYear = p.TotalExperienceInYear,
                                        EarliestJoinDate = p.EarliestJoinDate,
                                        SalaryExpectation = p.SalaryExpectation,
                                        Vehicle = p.Vehicle,
                                        JobSource = p.JobSource,
                                        NightShift = p.NightShift,
                                        IsReference = p.IsReference,
                                        ReferenceName = p.ReferenceName,
                                        ReferenceDesignation = p.ReferenceDesignation,
                                        ReferenceMobileNo = p.ReferenceMobileNo,
                                        EmailId = p.EmailId,
                                        OtherCertification = p.OtherCertification
                                    }).FirstOrDefault();
                    Candidate.PersonalInfo = perSonalInfo;

                    preEmployementDetail = (from p in db.InterPreEmpDetail
                                                //from p in getData.GetAllPreviousEmplpoyementDetails()
                                            where (p.CandidateId == id)
                                            select new InterPreEmpDetailViewModel
                                            {
                                                EmploymentId = p.EmploymentId,
                                                CandidateId = p.CandidateId,
                                                City = p.City,
                                                CompanyName = p.CompanyName,
                                                Designation = p.Designation,
                                                WorkFrom = p.WorkFrom,
                                                WorkTo = p.WorkTo,
                                                CtcMonth = p.CtcMonth
                                            }).ToList();
                    Candidate.PreviousEmploymentDetail = preEmployementDetail == null ? null : preEmployementDetail;


                    //preEmployementDetail = (
                    //    from p in getData.GetAllPreviousEmplpoyementDetails()
                    //    where (p.CandidateId == id)
                    //    select p
                    //    ).ToList();
                    //Candidate.PreviousEmploymentDetail = preEmployementDetail == null ? null : preEmployementDetail;
                    eduBackGround = (from p in db.InterEducBackground
                                     where (p.CandidateId == id)
                                     select new InterEducBackgroundViewModel()
                                     {
                                         EducationalId = p.EducationalId,
                                         CandidateId = p.CandidateId,
                                         BoardUniversityName = p.BoardUniversityName,
                                         CourseDegreeName = p.CourseDegreeName,
                                         PassingYear = p.PassingYear,
                                         GradePercentage = p.GradePercentage
                                     }).ToList();
                    Candidate.EducationBackground = eduBackGround;
                    //eduBackGround = (
                    //     from p in getData.GetAllEducationBackground()
                    //     where (p.CandidateId == id)
                    //     select p
                    //    ).ToList();
                    //Candidate.EducationBackground = eduBackGround;
                    reference = (from p in db.InterReference
                                 where (p.CandidateId == id)
                                 select new InterReferenceViewModel()
                                 {
                                     ReferenceId = p.ReferenceId,
                                     CandidateId = p.CandidateId,
                                     PersonName = p.PersonName,
                                     CompanyName = p.CompanyName,
                                     Designation = p.Designation,
                                     ContactNo = p.ContactNo
                                 }).ToList();
                    Candidate.Reference = reference;
                    Candidate.Language = new List<LanguageViewModel>();
                    Candidate.Language = (from l in db.InterLanguages
                                          where l.CandidateId == id
                                          select new LanguageViewModel
                                          {
                                              LanguageType = l.LanguageType,
                                              Read = l.Read,
                                              Write = l.Write,
                                              Speak = l.Speak
                                          }).ToList();

                    return View(Candidate);

                }
                else
                {
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                return View();
            }
        }

        [HttpGet]
        public JsonResult SaveComnets(int id,string txtcommnet)
        {
            JsonResult result = new JsonResult();
            try
            {
                InterPersonalInfo comment = db.InterPersonalInfo.Where(w => w.CandidateId == id).FirstOrDefault();
                comment.OtherComments = txtcommnet;
                db.SaveChanges();

                result.ContentType = "success";
                TempData["Success"] = "record saved successfully";
                result.Data = "record saved successfully";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ooops! something wrong try again";
                result.ContentType = "error";
                result.Data = "Ooops! something wrong try again";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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