using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace politiekeBarometer.Models
{
    public class HomeViewModel
    {
        public Dictionary<string, Chart> Charts { get; set; }
    }
}