using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;

namespace politiekeBarometer.Controllers
{
    public class PlatformController : Controller
    {
    IPlatformManager platformManager = new PlatformManager();
    IAppUserManager userManager = new ApplicationUserManager();
   
      
        // GET: Platform
        public ActionResult Index()
        {

            return View();
        }
      [HttpGet]
      public ActionResult Create()
      {
      ViewBag.Admins = userManager.GetSpecificRole("Admin");
      return View();
      }
   
    }
}