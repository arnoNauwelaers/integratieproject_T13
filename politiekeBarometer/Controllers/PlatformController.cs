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
    // GET: Platform/Create
    public ActionResult Create()
    {
      ViewBag.Admins = userManager.GetUsersFromRole("Admin");
      
      return View();
    }

    // POST: Platform/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      ViewBag.Admins = userManager.GetUsersFromRole("Admin");
      
      try
      {
        Platform p = new Platform();
        p.Admins = new List<ApplicationUser> { userManager.GetUser(Convert.ToString(collection["admin"])) };

        p.Name = Convert.ToString(collection["name"]);
        platformManager.AddPlatform(p);
        
        return RedirectToAction("Index");
      }
      catch(Exception e)
      {
        return View("Error: " + e);
      }
    }
    //GET: Platform/Delete
    public ActionResult Delete(int id)
        {
      Platform p = platformManager.GetPlatform(id);
            if (p.Equals(null))
            {
                return HttpNotFound();
            }
            return View(p);
        }
    // POST: Platform/Delete
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {

      try
      {
        Platform p = platformManager.GetPlatform(id);
        platformManager.RemovePlatform(p);
        return RedirectToAction("Index");

      }
      catch(Exception e)
      {
        Console.WriteLine(e.InnerException);
        return RedirectToAction("Index");
      }
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