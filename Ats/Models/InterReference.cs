using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class InterReference
    {
        [Key]
        public int ReferenceId { get; set; }
        public int CandidateId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string PersonName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string CompanyName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Designation { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ContactNo { get; set; }
    }
}