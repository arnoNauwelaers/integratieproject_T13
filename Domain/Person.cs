using System;
using System.Collections.Generic;

namespace BL.Domain
{
    public class Person : Item
    {
        public virtual ICollection<SocialMediaProfile> socialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual Organization Organization { get; set; }
        public Person()
        {
          typeInt = 1;
        }
    }
}
