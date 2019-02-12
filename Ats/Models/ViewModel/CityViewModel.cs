using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models.ViewModel
{
    public class CityViewModel
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityNamePresant { get; set; }
        public string CityNamePast { get; set; }
    }
}