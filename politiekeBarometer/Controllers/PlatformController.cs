using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using BL.Domain;
using BL.Managers;

namespace politiekeBarometer.Controllers
{
    public class PlatformController : Controller
    {
        PlatformManager platformManager;
        ApplicationUserManager userManager;

        public PlatformController()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.platformManager = new PlatformManager(unitOfWorkManager);
            this.userManager = new ApplicationUserManager(unitOfWorkManager);
        }

        // GET: Platform
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {

            List<Platform> platforms = platformManager.GetPlatforms().ToList();
            return View(platforms);

        }
        // GET: Platform/Create
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {

            ViewBag.Admins = userManager.GetUsersFromRole("Admin");

            return View();
        }

        // POST: Platform/Create
        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public ActionResult Create(FormCollection collection)
        {
            ViewBag.Admins = userManager.GetUsersFromRole("Admin");

            try
            {
                Platform p = new Platform
                {
                    Admins = new List<ApplicationUser> { userManager.GetUser(Convert.ToString(collection["admin"])) },
                    Name = Convert.ToString(collection["name"]),
                };
                platformManager.AddPlatform(p);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error: " + e);
            }
        }
        //GET: Platform/Delete
        [Authorize(Roles = "SuperAdmin")]
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
        [HttpPost, ActionName("Delete"), Authorize(Roles = "SuperAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                Platform p = platformManager.GetPlatform(id);
                platformManager.RemovePlatform(p);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Details(int id)
        {
            Platform p = platformManager.GetPlatform(id);
            ViewBag.Admins = p.Admins.ToList();
            return View(p);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(int id)
        {
            ViewBag.Platform = platformManager.GetPlatform(id);
            ViewBag.Admins = userManager.GetUsersFromRole("Admin").ToList();

            return View();
        }

        // POST: Profile/Edit
        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(FormCollection collection)
        {
            ViewBag.Admins = userManager.GetUsersFromRole("Admin").ToList();

            ViewBag.Platform = platformManager.GetPlatform(Convert.ToInt32(collection["id"]));
            try
            {
                Platform platform = platformManager.GetPlatform(Convert.ToInt32(collection["id"]));
                platform.Admins.Add(userManager.GetUser(Convert.ToString(collection["admin"])));
                platform.Name = Convert.ToString(collection["name"]);
                platformManager.ChangePlatform(platform);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet, Authorize(Roles = "SuperAdmin")]
        public ActionResult AddAdmin(int id)
        {
            ViewBag.Platform = platformManager.GetPlatform(id);
            ViewBag.Admins = userManager.GetUsersFromRole("Admin");
            return View();
        }
        [HttpPost, Authorize(Roles = "SuperAdmin")]
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


                return RedirectToAction("Details", new { id = platformId });
            }
            catch (Exception)
            {
                return View();
            }
        }

    }
}