using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GebruikersRepository
    {
        private static IList<User> Users = new List<User>();

        public User CreateUser(string username, string password, string email)
        {
            return new User() { Username = username, Password = password, Email = email };
        }

        public IEnumerable<User> ReadUsers()
        {
            return Users;
        }

        public User ReadUser(int id)
        {
            foreach (var u in Users)
            {
                if (u.Id == id) return u;
            }
            return null;
        }
    }
}
