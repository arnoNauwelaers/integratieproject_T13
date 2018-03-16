using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Alert
    {
        [Key]
        public int Id { get; set; }
        public String Type { get; set; }
        public String Conditie { get; set; }
        public String Parameter;
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }

        public Alert(string type, string parameter, int item1)
        {
            Type = type;
            Parameter = parameter;
            ItemId1 = item1;
        }
    }
}
