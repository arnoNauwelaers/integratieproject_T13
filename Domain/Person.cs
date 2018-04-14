using System;
using System.Collections.Generic;

namespace BL.Domain
{
    public class Person : Item
    {
        public virtual List<SocialMediaProfile> socialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public string FirstName { get; set; }
        public virtual Organization Organization { get; set; }
        public Person()
        {
        }
    }
}
