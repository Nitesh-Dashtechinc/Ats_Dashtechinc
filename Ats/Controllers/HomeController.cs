using Ats.Models;
using Ats.Models.ViewModel;
using Ats.Models.ViewModels;
using ExcelDataReader;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
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
                    feedback.IsFeddbackAdded = false;
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

        #region Bulk register form upload from excel file
        [HttpPost]
        public ActionResult BulkRegistration(List<HttpPostedFileBase> postedFiles)
        {
            foreach (HttpPostedFileBase postedFile in postedFiles)
            {
                Stream stream = postedFile.InputStream;

                IExcelDataReader reader = null;


                if (postedFile.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (postedFile.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View();
                }

                //  reader.IsFirstRowAsColumnNames = true;

                DataSet result = reader.AsDataSet();
                reader.Close();

                List<PreInterRegisterViewModel> lstObj = new List<PreInterRegisterViewModel>();
                if (result != null && result.Tables.Count > 0)
                {
                    if (result.Tables["PersonalInfo"] != null && result.Tables["PersonalInfo"].Rows.Count > 0)
                    {
                        List<InterPersonalInfo> lstPer = new List<InterPersonalInfo>();
                        for (int i = 0; i < result.Tables["PersonalInfo"].Rows.Count; i++)
                        {
                            if (i > 0)
                            {
                                PreInterRegisterViewModel obj = new PreInterRegisterViewModel();
                                InterPersonalInfo objPer = new InterPersonalInfo();

                                #region For PersonalInfo
                                objPer.FirstName = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][0]);
                                objPer.LastName = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][1]);
                                objPer.MobileNo1 = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][2]);
                                objPer.MobileNo2 = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][3]);
                                objPer.DateOfBirth = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][4]);
                                if (result.Tables["PersonalInfo"].Rows[i][5] != null && Convert.ToString(result.Tables["PersonalInfo"].Rows[i][5]) != "")
                                {
                                    objPer.Age = Convert.ToInt16(result.Tables["PersonalInfo"].Rows[i][5]);
                                }
                                objPer.Gender = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][6]);
                                objPer.MaritalStaus = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][7]);
                                if (result.Tables["PersonalInfo"].Rows[i][8] != null && Convert.ToString(result.Tables["PersonalInfo"].Rows[i][8]) != "")
                                {
                                    objPer.NoOfChildren = Convert.ToInt32(result.Tables["PersonalInfo"].Rows[i][8]);
                                }
                                objPer.AddressPresent = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][9]);
                                objPer.StatePresent = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][10]);
                                objPer.CityPresent = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][11]);
                                objPer.PincodePresent = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][12]);
                                objPer.AddressPast = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][13]);
                                objPer.StatePast = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][14]);
                                objPer.CityPast = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][15]);
                                objPer.PinCodePast = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][16]);
                                objPer.AppliedForDepartment = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][17]);
                                objPer.AppliedForDesignation = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][18]);
                                objPer.TotalExperienceInYear = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][19]);
                                objPer.EarliestJoinDate = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][20]);
                                objPer.SalaryExpectation = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][21]);
                                objPer.Vehicle = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][22]);
                                objPer.JobSource = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][23]);
                                if (result.Tables["PersonalInfo"].Rows[i][24] != null && Convert.ToString(result.Tables["PersonalInfo"].Rows[i][24]) != "")
                                {
                                    objPer.NightShift = Convert.ToBoolean(result.Tables["PersonalInfo"].Rows[i][24]);
                                }
                                if (result.Tables["PersonalInfo"].Rows[i][25] != null && Convert.ToString(result.Tables["PersonalInfo"].Rows[i][25]) != "")
                                {
                                    objPer.IsReference = Convert.ToBoolean(result.Tables["PersonalInfo"].Rows[i][25]);
                                }
                                objPer.ReferenceName = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][26]);
                                objPer.ReferenceMobileNo = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][27]);
                                objPer.ReferenceDesignation = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][28]);
                                objPer.EmailId = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][29]);
                                objPer.OtherCertification = Convert.ToString(result.Tables["PersonalInfo"].Rows[i][30]);
                                if (result.Tables["PersonalInfo"].Rows[i][31] != null && Convert.ToString(result.Tables["PersonalInfo"].Rows[i][31]) != "")
                                {
                                    objPer.CreatedDate = Convert.ToDateTime(result.Tables["PersonalInfo"].Rows[i][31]);
                                }
                                #endregion

                                obj.PersonalInfo = objPer;
                                lstObj.Add(obj);
                            }
                        }

                        if (lstObj != null && lstObj.Count > 0)
                        {
                            foreach (var item in lstObj)
                            {
                                #region For Languages
                                List<InterLanguage> lstLan = new List<InterLanguage>();
                                for (int i = 0; i < result.Tables["Languages"].Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        if(item.PersonalInfo.FirstName.ToLower() == Convert.ToString(result.Tables["Languages"].Rows[i][0]).ToLower()
                                            && item.PersonalInfo.LastName.ToLower() == Convert.ToString(result.Tables["Languages"].Rows[i][1]).ToLower())
                                        {
                                            InterLanguage objLan = new InterLanguage();
                                            objLan.LanguageType = Convert.ToString(result.Tables["Languages"].Rows[i][2]);
                                            objLan.Read = Convert.ToString(result.Tables["Languages"].Rows[i][3]);
                                            objLan.Speak = Convert.ToString(result.Tables["Languages"].Rows[i][4]);
                                            objLan.Write = Convert.ToString(result.Tables["Languages"].Rows[i][5]);
                                            lstLan.Add(objLan);
                                        }
                                    }
                                }
                                item.Languages = lstLan;
                                #endregion

                                #region For PreEmpDetail
                                List<InterPreEmpDetail> lstPreImp = new List<InterPreEmpDetail>();
                                for (int i = 0; i < result.Tables["PreEmpDetails"].Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        if (item.PersonalInfo.FirstName.ToLower() == Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][0]).ToLower()
                                            && item.PersonalInfo.LastName.ToLower() == Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][1]).ToLower())
                                        {
                                            InterPreEmpDetail objPreImp = new InterPreEmpDetail();
                                            objPreImp.CompanyName = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][2]);
                                            objPreImp.City = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][3]);
                                            objPreImp.Designation = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][4]);
                                            objPreImp.WorkFrom = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][5]);
                                            objPreImp.WorkTo = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][6]);
                                            objPreImp.CtcMonth = Convert.ToString(result.Tables["PreEmpDetails"].Rows[i][7]);
                                            lstPreImp.Add(objPreImp);
                                        }
                                    }
                                }
                                item.PreviousEmploymentDetail = lstPreImp;
                                #endregion

                                #region For References
                                List<InterReference> lstRef = new List<InterReference>();
                                for (int i = 0; i < result.Tables["References"].Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        if (item.PersonalInfo.FirstName.ToLower() == Convert.ToString(result.Tables["References"].Rows[i][0]).ToLower()
                                            && item.PersonalInfo.LastName.ToLower() == Convert.ToString(result.Tables["References"].Rows[i][1]).ToLower())
                                        {
                                            InterReference objRef = new InterReference();
                                            objRef.PersonName = Convert.ToString(result.Tables["References"].Rows[i][2]);
                                            objRef.CompanyName = Convert.ToString(result.Tables["References"].Rows[i][3]);
                                            objRef.Designation = Convert.ToString(result.Tables["References"].Rows[i][4]);
                                            objRef.ContactNo = Convert.ToString(result.Tables["References"].Rows[i][5]);
                                            lstRef.Add(objRef);
                                        }
                                    }
                                }
                                item.Reference = lstRef;
                                #endregion

                                #region For EducBackgrounds
                                List<InterEducBackground> lstEduBac = new List<InterEducBackground>();
                                for (int i = 0; i < result.Tables["EducBackgrounds"].Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        if (item.PersonalInfo.FirstName.ToLower() == Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][0]).ToLower()
                                            && item.PersonalInfo.LastName.ToLower() == Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][1]).ToLower())
                                        {
                                            InterEducBackground objEduBac = new InterEducBackground();
                                            objEduBac.BoardUniversityName = Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][2]);
                                            objEduBac.CourseDegreeName = Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][3]);
                                            objEduBac.PassingYear = Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][4]);
                                            objEduBac.GradePercentage = Convert.ToString(result.Tables["EducBackgrounds"].Rows[i][5]);
                                            lstEduBac.Add(objEduBac);
                                        }
                                    }
                                }
                                item.EducationBackground = lstEduBac;
                                #endregion

                                #region For EducBackgrounds
                                List<Feedback> lstFeedback = new List<Feedback>();
                                for (int i = 0; i < result.Tables["Feedbacks"].Rows.Count; i++)
                                {
                                    if (i > 0)
                                    {
                                        if (item.PersonalInfo.FirstName.ToLower() == Convert.ToString(result.Tables["Feedbacks"].Rows[i][0]).ToLower()
                                            && item.PersonalInfo.LastName.ToLower() == Convert.ToString(result.Tables["Feedbacks"].Rows[i][1]).ToLower())
                                        {
                                            Feedback objFeedback = new Feedback();
                                            if (Convert.ToString(result.Tables["Feedbacks"].Rows[i][2]) != "")
                                            {
                                                objFeedback.InterviewDate = Convert.ToDateTime(result.Tables["Feedbacks"].Rows[i][2]);
                                            }
                                            if (Convert.ToString(result.Tables["Feedbacks"].Rows[i][3]) != "")
                                            {
                                                objFeedback.CandidateStatus = Convert.ToBoolean(result.Tables["Feedbacks"].Rows[i][3]);
                                            }
                                            objFeedback.OtherComments = Convert.ToString(result.Tables["Feedbacks"].Rows[i][4]);
                                            if (Convert.ToString(result.Tables["Feedbacks"].Rows[i][5]) != "")
                                            {
                                                objFeedback.IsFeddbackAdded = Convert.ToBoolean(result.Tables["Feedbacks"].Rows[i][5]);
                                            }
                                            lstFeedback.Add(objFeedback);
                                        }
                                    }
                                }
                                item.Feedbacks = lstFeedback;
                                #endregion
                            }
                        }
                    }
                }

                #region Insert In Database
                if(lstObj !=null && lstObj.Count > 0)
                {
                    foreach (var obj in lstObj)
                    {
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

                            if (obj.Feedbacks != null)
                            {
                                foreach (Feedback l in obj.Feedbacks)
                                {
                                    l.CandidateId = id;
                                    db.Feedbacks.Add(l);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    JsonResult res = new JsonResult();
                    res.ContentType = "success";
                    res.Data = "Your Form Submited Successfully";
                    TempData["candidateId"] = 1;
                }
                #endregion
            }
            ViewBag.Message = "Data Update Successfully!";
            return RedirectToAction("Index");
        }
        #endregion

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

        //Generate Pdf from View Detail 
        public ActionResult PrintReport()
        {
            var report = new ActionAsPdf("ViewDetail");
            return report;
        }
        public ActionResult ReportById(int id)
        {
            try
            {
                GridPreInterRegisterViewModel Candidate = new GridPreInterRegisterViewModel();
                InterPersonalInfoViewModel perSonalInfo = new InterPersonalInfoViewModel();
                List<InterPreEmpDetailViewModel> preEmployementDetail = new List<InterPreEmpDetailViewModel>();
                List<InterEducBackgroundViewModel> eduBackGround = new List<InterEducBackgroundViewModel>();
                List<InterReferenceViewModel> reference = new List<InterReferenceViewModel>();
                List<LanguageViewModel> languages = new List<LanguageViewModel>();
                FeedbackViewModel feedbacks = new FeedbackViewModel();
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
                //Get Candidate feedback
                GetData getData = new GetData();
                feedbacks = (
                     from p in getData.GetCandidatefeedback()
                     where (p.CandidateId == id)
                     select p
                    ).FirstOrDefault();
                DateTime InterDate = Convert.ToDateTime(feedbacks.InterviewDate);
                string idate = String.Format("{0:dd/MM/yyyy}", InterDate);
                feedbacks.InterviewDate = idate;
                Candidate.Feedback = feedbacks;

                //feedbacks = (from f in db.Feedbacks
                //             where f.CandidateId == id
                //             select new FeedbackViewModel()
                //             {
                //                 CandidateId = f.CandidateId,
                //                 CandidateStatus = f.CandidateStatus,
                //                 InterviewDate = f.InterviewDate,
                //                 OtherComments = f.OtherComments
                //             }).ToList();
                //Candidate.Feedback = feedbacks;

                //var iDate = db.Feedbacks.Select(a => a.CandidateId == id).FirstOrDefault();
                //ViewBag.InterviewDate = Convert.ToDateTime(iDate);

                return View(Candidate);
            }
            catch (Exception ex)
            {
                ViewBag.errormessage = string.IsNullOrEmpty(Convert.ToString(ex.InnerException)) ? ex.Message.ToString() : ex.InnerException.ToString();
                return View();
            }
        }


        public ActionResult ViewDetailReport(int id)
        {
            var report = new ActionAsPdf("ReportById", new { id = id });
            return report;
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
                //TempData["candidateId"] = 1;
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
                    perSonalInfo = (from p in db.InterPersonalInfo
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
        public ActionResult GetFeedback(int Id)
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
                feedback.IsFeddbackAdded = true;
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

        public ActionResult CandidateDetailReport(int id)
        {
            Session["candidateId"] = id;
            return Redirect("/Reports/DashReport.aspx");
        }

        public List<InterPreEmpDetail> GetInterPreEmpDetail()
        {
            List<InterPreEmpDetail> preEmpDetails = new List<InterPreEmpDetail>();
            InterPreEmpDetail interPreEmp = new InterPreEmpDetail();
            for (int i = 1; i < 6; i++)
            {
                interPreEmp = new InterPreEmpDetail();
                interPreEmp.CandidateId = i;
                interPreEmp.CompanyName = "Dash";
                interPreEmp.City = "ahe";
                interPreEmp.WorkFrom = "02/03/2018";
                interPreEmp.WorkTo = "01/02/2819";
                preEmpDetails.Add(interPreEmp);
            }
            return preEmpDetails;
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