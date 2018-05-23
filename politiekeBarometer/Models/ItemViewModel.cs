using BL.Domain;
using System.Collections.Generic;

namespace politiekeBarometer.Models
{
    public class ItemViewModel
    {
        public List<Person> Persons { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Theme> Themes { get; set; }
        public string FileError { get; set; }
    }
}