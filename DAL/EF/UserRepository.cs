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
            ctx = BarometerDbContext.CreateContext();
            ctx.Database.Initialize(false);
        }

        public IEnumerable<User> ReadUsers()
        {
            return ctx.Users.Include(a => a.Alerts).ToList<User>();
        }


        public User CreateUser(User user)
        {
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public void UpdateUser(User user)
        {
            ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            User user = ctx.Users.Find(userId);
            ctx.Users.Remove(user);
            ctx.SaveChanges();
        }

        public User GetUser()
        {
            return ctx.Users.Include(a => a.Alerts).First();
        }
    }
}
