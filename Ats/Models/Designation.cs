using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string DesignationName { get; set; }
    }
}