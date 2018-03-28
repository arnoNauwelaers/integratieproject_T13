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

        public BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = new UserManager();
        }

        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = SocialMediaManager.CreatePosts();
        }

        /* GET: Basic
        public ActionResult Index()
        {
            return View();
        }*/
    }
}