using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using DAL.EF;

namespace politiekeBarometer.Controllers
{
  public class PageController : Controller
  {
    // GET: Page
    public ActionResult Person(int Id) {
        ItemManager im = new ItemManager();
        return View(im.ReadPerson(Id));
    }

    // GET: Page
    public ActionResult Organization(int Id) {
      ItemManager im = new ItemManager();
      return View(im.ReadOrganization(Id));
    }

    // GET: Page
    public ActionResult Theme(int Id) {
      ItemManager im = new ItemManager();
      return View(im.ReadTheme(Id));
    }

    public ActionResult Search(string Text) {
      int Id = 0;
      return RedirectToAction("Person", "Page", Id);
    }
  }
}