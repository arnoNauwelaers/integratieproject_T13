using System;
using System.Collections.Generic;

namespace BL.Domain
{
    public class Organization : Item
    {
        public virtual List<SocialMediaProfile> socialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual List<Person> persons { get; set; } = new List<Person>();
        public Organization()
        {
          typeInt = 2;
        }
    }
}
