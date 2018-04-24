using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Platform
    {
    private static int numberOfPlatform { get; set; } = 0;
        public int Id { get; set; }
        public String Name { get; set; }
        public ICollection<ApplicationUser> Admins { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<SocialMediaSource> Sources { get; set; }

    public Platform()
    {
      numberOfPlatform += 1;
      Id = numberOfPlatform;
    }
    }
}