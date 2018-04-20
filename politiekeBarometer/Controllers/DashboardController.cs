using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL.EF;
using BL;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace politiekeBarometer.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationUserManager UserManager;
        //private BarometerDbContext db = BarometerDbContext.Create();
        public DashboardController()
        {
            
        }
        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(UserManager.GetUser(User.Identity.GetUserId()));
        }
    }
}
