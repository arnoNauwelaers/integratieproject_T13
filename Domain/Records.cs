using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Domain
{
    public class Records
    {
        List<String> records { get; set; }

        public override string ToString()
        {
            String lokaal = "";
            foreach(var item in records)
            {
                lokaal += item.ToString() + "\n";
                
            }
            return lokaal;
        }
    }
}
