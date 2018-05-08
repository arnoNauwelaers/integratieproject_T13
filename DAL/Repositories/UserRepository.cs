using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL.EF;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private BarometerDbContext ctx;

        public UserRepository(BarometerDbContext ctx)
        {
            this.ctx = ctx;
        }

        public List<ApplicationUser> ReadUsers()
        {
            return ctx.Users.Include(a => a.Alerts).ToList<ApplicationUser>();
        }

    public List<ApplicationUser> ReadUsersFromRole(string roleId)
    {
      List<ApplicationUser> usersInRole = ctx.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();
      return usersInRole;
    }

    public List<ApplicationUser> ReadUsersWithoutRole(string roleId)
    {
      List<ApplicationUser> usersInRole = ctx.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();
      return usersInRole;
    }


    public ApplicationUser CreateUser(ApplicationUser user)
        {
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public void UpdateUser(ApplicationUser user)
        {
            ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteUser(ApplicationUser user)
    {
      ctx.Users.Remove(user);
      ctx.SaveChanges();
    }

        public void DeleteUser(int userId)
        {
            ApplicationUser user = ctx.Users.Find(userId);
            ctx.Users.Remove(user);
            ctx.SaveChanges();
        }

      

        public ApplicationUser ReadUser(string id = "")
        {
            if (!id.Equals(""))
            {
                return ctx.Users.Include(a => a.Alerts).Where(u => u.Id == id).First();
            }
            else
            {
                return null;
            }
        }

    public ApplicationUser ReadUserByName(string name)
    {
      return ctx.Users.Single(u => u.UserName.Equals(name));
    }

    public List<IdentityRole> ReadRoles()
    {
      return ctx.Roles.ToList();
    }

    public List<IdentityRole> ReadSpecificRole(string roleName)
    {
      return ctx.Roles.Where(r => r.Name.ToUpper().Equals(roleName.ToUpper())).ToList();
    }

    public List<IdentityRole> ReadRolesWithout(string partRoleName)
    {
      return ctx.Roles.Where(r => !r.Name.Contains(partRoleName)).ToList();
    }

    public string ReadRoleId(string roleName)
    {
      IdentityRole role = ctx.Roles.Where(r => r.Name.ToUpper().Equals(roleName.ToUpper())).First();
      return role.Id;
    }

    public ApplicationUser GetUserByToken(string token) {
      if(ctx.Users.Any(u => u.AppToken == token)) {
        return ctx.Users.First(u => u.AppToken == token);
      }else{
        return null;
      }
    }
  }
}
