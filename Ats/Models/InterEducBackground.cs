using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class InterEducBackground
    {
        [Key]
        public int EducationalId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string BoardUniversityName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string CourseDegreeName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string PassingYear { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string GradePercentage { get; set; }
    }
}