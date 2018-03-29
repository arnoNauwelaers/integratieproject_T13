using BL;
using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace politiekeBarometer.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //TODO: delete code
            //BasicController controller = new BasicController();
            //controller.SynchronizeDatabase();
            //ItemController.SynchroniseDb();
            List<Notification> model = new List<Notification>();
            //foreach(Notification n in ItemController.notificationManager.GetNotifications()) {
            //    if(n.User.Equals(ItemController.gebruikerManager.GetUser()))
            //    {
            //        model.Add(n);
            //    }
            //}
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