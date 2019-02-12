using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ats.Models
{
    public class InterPreEmpDetail
    {
        [Key]
        public int EmploymentId { get; set; }
        public int CandidateId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string CompanyName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string City { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string Designation { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string WorkFrom { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string WorkTo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        public string CtcMonth { get; set; }
    }
}