using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Person : Item
    {
        public List<SocialMediaProfile> socialMediaProfiles { get; set; }
        public string FirstName { get; set; }
        public Organization Organization { get; set; }
        public Person()
        {
            socialMediaProfiles = new List<SocialMediaProfile>();
        }
    }
}
