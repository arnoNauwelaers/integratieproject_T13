using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class Theme : Item
    {
        public Theme(int id, string name) : base(id, name)
        {
        }

        List<String> KeyWords { get; set; }
    }
}
