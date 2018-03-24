using BL.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserManager
    {
        private AlertRepository AlertRepository;

        public UserManager()
        {
            AlertRepository = new AlertRepository();
        }
    }
}
