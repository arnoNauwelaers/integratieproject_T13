using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL.EF;

namespace BL
{
  public class AppRoleManager : RoleManager<IdentityRole>
  {
    public AppRoleManager() : base(new RoleStore<IdentityRole>(new BarometerDbContext()))
    {

    }
  }
}
