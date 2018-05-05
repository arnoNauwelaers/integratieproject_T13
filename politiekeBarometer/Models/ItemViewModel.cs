using BL.Domain;
using System.Collections.Generic;

namespace politiekeBarometer.Models
{
    public class ItemViewModel
    {
        //TODO: item list public List<Item> Items { get; set; }
        public List<Person> Persons { get; set; }
        public List<Organization> organizations { get; set; }
        public List<Theme> themes { get; set; }
        public string fileError { get; set; }
    }
}