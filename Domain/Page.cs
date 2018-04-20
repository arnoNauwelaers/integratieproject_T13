using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public abstract class Page
    {
        public int PageId { get; set; }
        public virtual ICollection<Zone> Zones { get; set; }

    }
}