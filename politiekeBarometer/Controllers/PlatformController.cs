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

    public ActionResult Details(int id)
    {
      Platform p = platformManager.GetPlatform(id);
      ViewBag.Admins = p.Admins;
      return View(p);
    }

    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      ViewBag.Platform = platformManager.GetPlatforms();
      ViewBag.Admins = userManager.GetUsersFromRole("Admin");
      try
      {
        int platformId = Convert.ToInt32(collection["platformid"]);
        Platform p = platformManager.GetPlatform(platformId);
        p.Name = Convert.ToString(collection["platformnaam"]);
        p.Admins.Add(userManager.GetUser(Convert.ToString(collection["admin"])));
        platformManager.ChangePlatform(p);
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

  }
}