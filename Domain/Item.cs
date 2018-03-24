using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Item
    {
        [Key]
        public int itemId { get; set; }
        public string name { get; set; }
        //public int photo { get; set; }
        //public ICollection<Set> sets { get; set; }

        //public ICollection<Set> Set { get; set; }
        public ICollection<Alert> Alert { get; set; }
    }
}
