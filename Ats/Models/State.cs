using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string StateName { get; set; }
    }
}