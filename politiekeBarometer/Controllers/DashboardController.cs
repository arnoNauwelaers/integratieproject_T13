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
        private ApplicationUserManager userManager;
        private ItemManager itemManager;
        private ChartManager ChartManager;
        //private BarometerDbContext db = BarometerDbContext.Create();
        public DashboardController()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.userManager = new ApplicationUserManager(unitOfWorkManager);
            ChartManager = new ChartManager(unitOfWorkManager);
            itemManager = new ItemManager(unitOfWorkManager);
        }
        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user = userManager.GetUser(User.Identity.GetUserId());
            List<Person> Persons = (List<Person>)itemManager.GetPersons();
            List<Theme> Themes = (List<Theme>)itemManager.GetThemes();
            List<Organization> Organizations = (List<Organization>)itemManager.GetOrganizations();
            DashboardModel Model = new DashboardModel
            {
                Persons = Persons,
                Themes = Themes,
                Organizations = Organizations,
                Charts = userManager.GetUser(User.Identity.GetUserId()).Dashboard
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
            ApplicationUser user = userManager.GetUser(User.Identity.GetUserId());
            Chart chart = ChartManager.CreateChartFromDashboard(items, type, value, frequency);
            user.Dashboard.Add(chart);
            userManager.Update(user);
            return RedirectToAction("Index");
        }

        [HttpPost, Authorize]
        public ActionResult DeleteChart(int id)
        {
            Chart chart = ChartManager.GetChart(id, false);
            ApplicationUser user = userManager.GetUser(User.Identity.GetUserId());
            if (user.Dashboard.Contains(chart))
            {
                user.Dashboard.Remove(chart);
                userManager.Update(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost, Authorize]
        public ActionResult SaveChart(int id)
        {
            Chart chart = ChartManager.GetChart(id, false);
            chart.Saved = true;
            ApplicationUser user = userManager.GetUser(User.Identity.GetUserId());
            userManager.Update(user);
            return RedirectToAction("Index");
        }

        [HttpPost, Authorize]
        public ActionResult EditChart(int chartid, string items, string type, string frequency)
        {
            ChartManager.EditChartFromDashboard(chartid, items, type, frequency);
            return RedirectToAction("Index");
        }

        [HttpGet, Authorize]
        public ActionResult MoveToDashboard(int id)
        {
            Chart chart = ChartManager.GetChart(id, false);
            ApplicationUser user = userManager.GetUser(User.Identity.GetUserId());
            user.Dashboard.Add(new Chart() { ChartType = chart.ChartType, ChartValue = chart.ChartValue, FrequencyType = chart.FrequencyType, Zone = (new Zone() { Width = 2.43, X = 10, Y = 10 }) });
            userManager.Update(user);
            return RedirectToAction("Index");
        }
    }
}
