using BL.Domain;
using System.Collections.Generic;

namespace politiekeBarometer.Models
{
    public class DashboardModel
    {
        public IEnumerable<Chart> Charts { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public IEnumerable<Theme> Themes { get; set; }
        public IEnumerable<Organization> Organizations { get; set; }
    }
}