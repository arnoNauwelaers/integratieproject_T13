using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Person : Item
    {
        public int PersonID { get; set; }
        public ICollection<SocialMediaProfile> socialMediaProfiles { get; set; }

        public Organization Organization { get; set; }
    }
}
