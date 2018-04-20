using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Platform
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public List<ApplicationUser> Admins { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<SocialMediaSource> Sources { get; set; }
    }
}