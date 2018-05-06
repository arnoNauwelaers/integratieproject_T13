using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();


        //TODO delete
        //public int photo { get; set; }
        //public ICollection<Set> sets { get; set; }

        [NotMapped]
        public int TypeInt { get; set; }

        public Item()
        {
          TypeInt = 0;
          //person = 1
          //organisation = 2
          //theme = 3
        }
    }
}
