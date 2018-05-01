using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Theme : Item
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public ICollection<string> Keywords { get; set; } = new List<string>();

        public Theme(string val)
        {
            Value = val;
            typeInt = 3;
        }

        public Theme()
        {
            typeInt = 3;
        }
    }
}