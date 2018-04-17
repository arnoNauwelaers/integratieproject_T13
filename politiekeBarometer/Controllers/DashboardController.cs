using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BL.Domain;
using DAL.EF;

namespace politiekeBarometer.Controllers
{
    public class DashboardController : Controller
    {
        //private BarometerDbContext db = BarometerDbContext.Create();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}
