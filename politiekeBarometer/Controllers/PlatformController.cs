using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using BL.Domain;

namespace politiekeBarometer.Controllers
{
    public class PlatformController : Controller
    {
    IPlatformManager platformManager = new PlatformManager();
    IAppUserManager userManager = new ApplicationUserManager();
   
      
        // GET: Platform
        public ActionResult Index()
        {
      List<Platform> platforms = platformManager.GetPlatforms().ToList();
      return View(platforms);
      
        }
      [HttpGet]
      public ActionResult Create()
      {
      ViewBag.Admins = userManager.GetSpecificRole("Admin");
      return View();
      }

     public ActionResult Delete(int id)
        {
      Platform p = platformManager.GetPlatform(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(id);
        }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      platformManager.RemovePlatform(id);
      return RedirectToAction("Index");
    }

  }
}