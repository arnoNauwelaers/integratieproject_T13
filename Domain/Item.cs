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
        public int Id { get; set; }
        public String Name { get; set; }

        public Item(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
