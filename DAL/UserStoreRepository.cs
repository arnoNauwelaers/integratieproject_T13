using BL.Domain;
using DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserStoreRepository : UserStore<ApplicationUser>
    {
        public UserStoreRepository() : base((BarometerDbContext.Create()))
        {
            
        }
    }
}
