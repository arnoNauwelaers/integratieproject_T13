using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Organization : Item
    {
        public List<SocialMediaProfile> socialMediaProfiles { get; set; }
        public List<Person> persons { get; set; }
        public Organization()
        {
            persons = new List<Person>();
            socialMediaProfiles = new List<SocialMediaProfile>();
        }
    }
}
