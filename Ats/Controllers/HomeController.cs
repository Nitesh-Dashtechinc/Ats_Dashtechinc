using Ats.Models;
using Ats.Models.ViewModel;
using Ats.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                                               join f in db.Feedbacks on e.CandidateId equals f.CandidateId
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
                                                   CityPresent = e.CityPresent,
                                                   InterviewDate = f.InterviewDate,
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
        //Add new candidate 
        public JsonResult SavePreInterView(PreInterRegisterViewModel obj)
        {
            JsonResult res = new JsonResult();
            try
            {
                //throw null;
                DateTime now = DateTime.Now;
               // obj.PersonalInfo.InterviewDate = now;
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
                    Feedback feedback = new Feedback();
                    feedback.CandidateId = id;
                    feedback.CandidateStatus = false;
                    feedback.InterviewDate = now;
                    feedback.OtherComments = "";
                    db.Feedbacks.Add(feedback);
                    db.SaveChanges();

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
        //Detailview of candidate
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
                List<LanguageViewModel> languages = new List<LanguageViewModel>();
                CityViewModel cities = new CityViewModel();
                StateViewModel states = new StateViewModel();
                DepartmentViewModel departments = new DepartmentViewModel();
                DesignationViewModel designations = new DesignationViewModel();
                perSonalInfo =                     (                    from p in db.InterPersonalInfo                    where (p.CandidateId == id)                    select new InterPersonalInfoViewModel()
                    {                        CandidateId = p.CandidateId,                        FirstName = p.FirstName,                        LastName = p.LastName,                        MobileNo1 = p.MobileNo1,                        MobileNo2 = p.MobileNo2,                        DateOfBirth = p.DateOfBirth,                        Age = p.Age,                        Gender = p.Gender,                        MaritalStaus = p.MaritalStaus,                        NoOfChildren = p.NoOfChildren,                        AddressPresent = p.AddressPresent,                        StatePresent = p.StatePresent,                        CityPresent = p.CityPresent,                        PincodePresent = p.PincodePresent,                        AddressPast = p.AddressPast,                        StatePast = p.StatePast,                        CityPast = p.CityPast,                        PinCodePast = p.PinCodePast,                        AppliedForDepartment = p.AppliedForDepartment,                        AppliedForDesignation = p.AppliedForDesignation,                        TotalExperienceInYear = p.TotalExperienceInYear,                        EarliestJoinDate = p.EarliestJoinDate,                        SalaryExpectation = p.SalaryExpectation,                        Vehicle = p.Vehicle,                        JobSource = p.JobSource,                        NightShift = p.NightShift,                        IsReference = p.IsReference,                        ReferenceName = p.ReferenceName,                        ReferenceDesignation = p.ReferenceDesignation,                        ReferenceMobileNo = p.ReferenceMobileNo,                        EmailId = p.EmailId,                        OtherCertification = p.OtherCertification,                        ///OtherComments=p.OtherComments,                        //CandidateStatus = p.CandidateStatus,                        //InterviewDate = p.InterviewDate
                    }).FirstOrDefault();

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
                //Candidate.Language = new List<LanguageViewModel>();

                languages = (from l in db.InterLanguages
                                      where l.CandidateId == id
                                      select new LanguageViewModel
                                      {
                                          LanguageType = l.LanguageType,
                                          Read = l.Read,
                                          Write = l.Write,
                                          Speak = l.Speak
                                      }).ToList();
                Candidate.Language = languages;
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
        //Preview detail after new candidate added
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
                    List<LanguageViewModel> languages = new List<LanguageViewModel>();
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
                                        MaritalStaus = p.MaritalStaus,
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
                    languages = (from l in db.InterLanguages
                                 where l.CandidateId == id
                                 select new LanguageViewModel
                                 {
                                     LanguageType = l.LanguageType,
                                     Read = l.Read,
                                     Write = l.Write,
                                     Speak = l.Speak
                                 }).ToList();
                    Candidate.Language = languages;

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

        //home/GetFeedback
        [HttpGet]
        public ActionResult GetFeedback(int Id )
        {
            try
            {             
                FeedbackViewModel feedbackView = new FeedbackViewModel();
                Feedback feedback = db.Feedbacks.Where(f => f.CandidateId == Id).FirstOrDefault();
                feedbackView.CandidateId = feedback.CandidateId;
                feedbackView.CandidateStatus = feedback.CandidateStatus;
                feedbackView.InterviewDate = Convert.ToString(feedback.InterviewDate);                
                ViewBag.InDate = Convert.ToDateTime(feedback.InterviewDate);
                feedbackView.OtherComments = feedback.OtherComments;            
                return PartialView(feedbackView);
            }
            catch (Exception ex)
            {              
                return null;
            }
        }
        //home/Postfeedback
        [HttpPost]
        public JsonResult PostFeedback(FeedbackViewModel feedbackView)
        {
            JsonResult result = new JsonResult();
            try
            {
                Feedback feedback = db.Feedbacks.Where(f => f.CandidateId == feedbackView.CandidateId).FirstOrDefault();
                feedback.CandidateId = feedbackView.CandidateId;
                feedback.CandidateStatus = feedbackView.CandidateStatus;          
                feedback.InterviewDate = DateTime.ParseExact(feedbackView.InterviewDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                feedback.OtherComments = feedbackView.OtherComments;
                db.SaveChanges();
                result.Data = "record saved successfully";
                result.ContentType = "success";
                return result;
            }
            catch (Exception)
            {
                result.ContentType = "error";                
                return Json(result.ContentType, JsonRequestBehavior.AllowGet);
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