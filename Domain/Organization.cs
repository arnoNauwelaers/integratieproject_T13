using System;
using System.Collections.Generic;

namespace BL.Domain
{
    public class Organization : Item
    {
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
<<<<<<< HEAD
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();
=======
        public virtual ICollection<Person> persons { get; set; } = new List<Person>();
>>>>>>> acb45e09462bf77bba1280c5e70224fd53d206ea

        public Organization()
        {
          TypeInt = 2;
        }
    }
}
