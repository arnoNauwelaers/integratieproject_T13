using BL.Domain;
using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AlertManager
    {
        private AlertRepository repo;

        public AlertManager()
        {
            repo = new AlertRepository();
        }

        //private Boolean SendMailToAll(List<User> users)
        //{
        //    return false;
        //}

        public Alert getAlert(int id)
        {
            return repo.getAlert(id);
        }

    }
}
