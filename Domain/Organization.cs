using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class Organization : Item
    {
        public ICollection<SocialMediaProfile> socialMediaProfiles { get; set; }
        public ICollection<Person> persons { get; set; }
    }
}
