using BL.Domain;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace politiekeBarometer.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            List<Notification> model = new List<Notification>();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}