using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        // amount of minutes between API calls
        public int Interval { get; set; }
        public virtual ICollection<ApplicationUser> Admins { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<SocialMediaSource> Sources { get; set; }

    }
}