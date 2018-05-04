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
        p.Interval = Convert.ToInt32(collection["interval"]);
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
      ViewBag.Admins = p.Admins.ToList();
      return View(p);
    }


    public ActionResult Edit(int id)
    {
      ViewBag.Platform = platformManager.GetPlatform(id);
      ViewBag.Admins = userManager.GetUsersFromRole("Admin").ToList();

      return View();
    }
    
    // POST: Profile/Edit
    [HttpPost]
    public ActionResult Edit(FormCollection collection)
    {
      ViewBag.Admins = userManager.GetUsersFromRole("Admin").ToList();
      
      ViewBag.Platform = platformManager.GetPlatform(Convert.ToInt32(collection["id"]));
      try
      {
        Platform platform = platformManager.GetPlatform(Convert.ToInt32(collection["id"]));
        platform.Admins.Add(userManager.GetUser(Convert.ToString(collection["admin"])));
        platform.Name = Convert.ToString(collection["name"]);
        platform.Interval = Convert.ToInt32(collection["interval"]);
        platformManager.ChangePlatform(platform);

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    [HttpGet]
    public ActionResult AddAdmin(int id)
    {
      ViewBag.Platform = platformManager.GetPlatform(id);
      ViewBag.Admins = userManager.GetUsersFromRole("Admin");
      return View();
    }
    [HttpPost]
    public ActionResult AddAdmin(FormCollection collection)
    {
      ViewBag.Admins = userManager.GetUsersFromRole("Admin");
      try
      {
        int platformId = Convert.ToInt32(collection["id"]);
        string adminId = Convert.ToString(collection["admin"]);
        Platform p = platformManager.GetPlatform(platformId);
        ApplicationUser a = userManager.GetUser(adminId);
        p.Admins.Add(a);
        platformManager.ChangePlatform(p);
        

        return RedirectToAction("Details", new { id = platformId});
      }
      catch (Exception)
      {
        return View();
      }
    }

  }
}