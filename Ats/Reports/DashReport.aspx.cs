using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ats.Models;
using Ats.Models.ViewModel;
using Ats.Models.ViewModels;
using Microsoft.Reporting.WebForms;
namespace Ats.Reports
{
    public partial class DashReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = (int)Session["candidateId"];
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            GridPreInterRegisterViewModel Candidate = new GridPreInterRegisterViewModel();           
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Report1.rdlc");
            ApplicationDbContext db = new ApplicationDbContext();     
            // Specify report's dataset name and the records it use
            var datasource = new ReportDataSource("InterPersonalInfoes", (from p in db.InterPersonalInfo where p.CandidateId == id
                                                                          select p));
            ReportViewer1.LocalReport.DataSources.Clear(); 
            ReportViewer1.LocalReport.DataSources.Add(datasource);
        }
    }
}