using BL.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class GebruikerManager
    {
        private GebruikerRepository repo;

        public GebruikerManager(User user)
        {
            this.repo = new GebruikerRepository();
            SetUser(user);
        }

        public void SetUser(User user)
        {
            repo.SetUser(user);
        }

        public User GetUser()
        {
            return repo.GetUser();
        }

        public void SendMail()
        {

        }
    }
}
