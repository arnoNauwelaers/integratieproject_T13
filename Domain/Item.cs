using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        //public int photo { get; set; }
        //public ICollection<Set> sets { get; set; }

        //public ICollection<Set> Set { get; set; }
        public List<Alert> Alerts { get; set; }
        public Item()
        {
            Alerts = new List<Alert>();
        }
    }
}
