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
using Newtonsoft.Json.Linq;
using politiekeBarometer.Models;
using BL.Managers;

namespace politiekeBarometer.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationUserManager UserManager;
        private ChartManager ChartManager;
        //private BarometerDbContext db = BarometerDbContext.Create();
        public DashboardController()
        {
            ChartManager = new ChartManager();
        }
        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
            List<Item> Items = UserManager.GetItemsFromUser(user);
            List<Person> Persons = new List<Person>();
            List<Theme> Themes = new List<Theme>();
            List<Organization> Organizations = new List<Organization>();
            foreach (var item in user.FollowedItems)
            {
                if (item.TypeInt == 1)
                {
                    Persons.Add((Person)item);
                }
                else if (item.TypeInt == 2)
                {
                    Themes.Add((Theme)item);
                }
                else if (item.TypeInt == 3)
                {
                    Organizations.Add((Organization)item);
                }
            }
            DashboardModel Model = new DashboardModel
            {
                Persons = Persons,
                Themes = Themes,
                Organizations = Organizations,
                Charts = UserManager.GetUser(User.Identity.GetUserId()).Dashboard
            };
            foreach (var chart in Model.Charts)
            {
                ChartManager.RetrieveDataChart(chart);
            }
            return View(Model);
        }

        [HttpPost, Authorize]
        public ActionResult AddChart(string items, string type, string value, string frequency)
        {
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
            Chart chart = ChartManager.CreateChartFromDashboard(items, type, value, frequency);
            user.Dashboard.Add(chart);
            UserManager.Update(user);
            return RedirectToAction("Index");
        }

        [HttpPost, Authorize]
        public ActionResult DeleteChart(int id)
        {
            Chart chart = ChartManager.GetChart(id);
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
            if (user.Dashboard.Contains(chart))
            {
                user.Dashboard.Remove(chart);
                UserManager.Update(user);
            }
            return RedirectToAction("Index");
        }
    }
}
