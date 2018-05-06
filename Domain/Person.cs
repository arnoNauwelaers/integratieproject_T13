using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Person : Item
    {
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual Organization Organization { get; set; }

        //TODO delete
        public Person(string val)
        {
            Name = val;
            TypeInt = 1;
        }

        public Person()
        {
            TypeInt = 1;
        }
    }
}
