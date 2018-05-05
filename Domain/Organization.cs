using System;
using System.Collections.Generic;

namespace BL.Domain
{
    public class Organization : Item
    {
        public virtual ICollection<SocialMediaProfile> socialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual ICollection<Person> persons { get; set; } = new List<Person>();

        public Organization()
        {
          typeInt = 2;
        }
    }
}
