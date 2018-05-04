﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace BL.Domain
{
  public class ApplicationUser : IdentityUser
  {
    //public bool Admin { get; set; }
    //public string UserName { get; set; }
    //public string Password { get; set; }
    public bool Geverifieerd { get; set; }
    public bool Google { get; set; }
    public bool Facebook { get; set; }
    public int AantalAanmeldingen { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public int TijdActief { get; set; }
    public virtual ICollection<Item> followedItems { get; set; } = new List<Item>();
    public virtual ICollection<Chart> Dashboard { get; set; } = new List<Chart>();

    /*[Required]
    public ICollection<Deelplatform> Deelplatformen { get; set; }*/
    /*[Required]
    public ICollection<Dashboard> Dashboards { get; set; }*/
    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ApplicationUser()
    {
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

      // Add custom user claims here
      return userIdentity;
    }

  }
}
