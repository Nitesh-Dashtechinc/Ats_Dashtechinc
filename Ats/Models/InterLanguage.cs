using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class InterLanguage
    {
        [Key]
        public int LanguageId { get; set; }
        public int CandidateId { get; set; }
        public string LanguageType { get; set; }
        public string Read { get; set; }
        public string Speak { get; set; }
        public string Write { get; set; }
    }
}