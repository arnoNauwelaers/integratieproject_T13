using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Person : Item
    {
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual Organization Organization { get; set; }

        public Person(string val)
        {
            Name = val;
        }

        public Person()
        {
        }
    }
}
