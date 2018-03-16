using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GebruikerRepository
    {
        private User user;

        public GebruikerRepository()
        {
        }

        public void SetUser(User user)
        {
            this.user = user;
        }
        public User GetUser()
        {
            return this.user;
        }
    }
}
