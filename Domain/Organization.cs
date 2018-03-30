using System;
using System.Collections.Generic;

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
