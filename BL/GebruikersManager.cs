using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BL.Domain;

namespace BL
{
    public class GebruikersManager
    {
        private GebruikersRepository repo;

        public GebruikersManager()
        {
            this.repo = new GebruikersRepository();
        }

        public User AddUser(string username, string password, string email)
        {
            return repo.CreateUser(username, password, email);
        }

        public User GetUser(int id)
        {
            return repo.ReadUser(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return repo.ReadUsers();
        }
    }
}
