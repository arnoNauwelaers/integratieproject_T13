using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using BL.Domain;

namespace BL
{
  public interface IAppUserManager
  {
    List<IdentityRole> GetRoles();
    List<IdentityRole> GetRolesWithout(string partRoleName);
    List<IdentityRole> GetSpecificRole(string roleName);
    List<ApplicationUser> GetUsersFromRole(string roleName);
    ApplicationUser GetUser(string id);
    List<ApplicationUser> GetUsers();
  }
}
