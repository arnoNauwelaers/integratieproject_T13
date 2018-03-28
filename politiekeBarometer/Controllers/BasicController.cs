using BL;
using BL.Domain;
using System.Collections.Generic;
using System.Web.Mvc;

namespace politiekeBarometer.Controllers
{
    public class BasicController : Controller
    {
        private SocialMediaManager SocialMediaManager;
        private UserManager UserManager;

        //TODO: Constructor protected
        public BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = new UserManager(SocialMediaManager);
        }

        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = SocialMediaManager.CreatePosts();
            List<Alert> alerts = new List<Alert>();
            foreach (var item in alteredItems)
            {
                alerts = UserManager.GetAlerts(item);
                foreach(var alert in alerts)
                {
                    UserManager.InspectAlert(alert);
                }
            }

        }

        /* GET: Basic
        public ActionResult Index()
        {
            return View();
        }*/
    }
}