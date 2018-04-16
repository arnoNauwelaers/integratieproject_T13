using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class UserRepository
    {
        private BarometerDbContext ctx;

        public UserRepository()
        {
            ctx = BarometerDbContext.Create();
            ctx.Database.Initialize(false);
        }

        public IEnumerable<ApplicationUser> ReadUsers()
        {
            return ctx.Users.Include(a => a.Alerts).ToList<ApplicationUser>();
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

        public void DeleteUser(int userId)
        {
            ApplicationUser user = ctx.Users.Find(userId);
            ctx.Users.Remove(user);
            ctx.SaveChanges();
        }

        public ApplicationUser GetUser(string id = "")
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
    }
}
