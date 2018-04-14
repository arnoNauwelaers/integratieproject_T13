using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        //public int photo { get; set; }
        //public ICollection<Set> sets { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
        public Item()
        {
        }
    }
}
