using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BL.Domain
{
    public class User : IdentityUser
    {
        public bool Admin { get; set; }
        //public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public bool Geverifieerd { get; set; }
        public bool Google { get; set; }
        public bool Facebook { get; set; }
        public int AantalAanmeldingen { get; set; }
        public int TijdActief { get; set; }

        /*[Required]
        public ICollection<Deelplatform> Deelplatformen { get; set; }*/
        /*[Required]
        public ICollection<Dashboard> Dashboards { get; set; }*/
        public virtual List<Alert> Alerts { get; set; } = new List<Alert>();
        public User()
        {
        }

    }
}
