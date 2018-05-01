using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Person : Item
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual Organization Organization { get; set; }
        public Person(string val)
        {
            Value = val;
            typeInt = 1;
        }

        public Person()
        {
            typeInt = 1;
        }
    }
}
