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
using BL.Domain;

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
            List<Item> Items = UserManager.GetItemsFromUser(User.Identity.GetUserId());
            List<Person> Persons = new List<Person>();
            List<Theme> Themes = new List<Theme>();
            List<Organization> Organizations = new List<Organization>();
            foreach (var item in Items)
            {
                if (item.typeInt == 1)
                {
                    Persons.Add((Person)item);
                }
                else if (item.typeInt == 2)
                {
                    Themes.Add((Theme)item);
                }
                else if (item.typeInt == 3)
                {
                    Organizations.Add((Organization)item);
                }
            }
            ViewBag.Persons = Persons;
            ViewBag.Themes = Themes;
            ViewBag.Organizations = Organizations;
            return View(UserManager.GetUser(User.Identity.GetUserId()));
        }
    }
}
