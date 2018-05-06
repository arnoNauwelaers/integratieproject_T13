using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Theme : Item
    {
        public ICollection<Keyword> Keywords { get; set; } = new List<Keyword>();

        public Theme(string val)
        {
            base.Name = val;
            TypeInt = 3;
        }

        public Theme()
        {
            TypeInt = 3;
        }
    }
}