using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BL;
using DAL.EF;
using System.Web.Helpers;
using BL.Domain;

namespace politiekeBarometer.Controllers
{
  
  public class PageController : Controller
  {
    IItemManager im = new ItemManager();
    ApplicationUserManager userManager = new ApplicationUserManager();
    // GET: Page
    public ActionResult Person(int id) {
        
      Person p = im.ReadPerson(id);
      SocialMediaManager sm = new SocialMediaManager();
      ChartManager cm = new ChartManager();
      List<Item> items = new List<Item>();
      items.Add(p);

      
      //System.Web.Helpers.Chart testChart = new System.Web.Helpers.Chart(width: 600, height: 400).AddTitle("Test").AddSeries("Default",chartType: "Column", xValue:)
        return View(p);
        
    }

    // GET: Page
    public ActionResult Organization(int Id) {
      return View(im.ReadOrganization(Id));
    }

    public ActionResult OrganizationOverview()
    {
      List<Organization> orgs = im.GetOrganizations();
      ViewBag.Organizations = orgs;
      return View();

    }

    public ActionResult PersonOverview()
    {
      List<Person> pers = im.GetPersons();
      ViewBag.Persons = pers;
      return View();
    }

    // GET: Page
    public ActionResult Theme(int Id) {
      
      return View(im.ReadTheme(Id));
    }

    public ActionResult AddItem(FormCollection collection)
    {
      ApplicationUser u = userManager.GetUser(System.Web.HttpContext.Current.User.Identity.GetUserId());
      Item i = im.GetItem(Convert.ToInt32(collection["id"]));
      u.followedItems.Add(i);
      userManager.ChangeUser(u);
      if (i.typeInt == 1) { return RedirectToAction("Person", new { id = i.ItemId }); }
      else {
        return RedirectToAction(i.typeInt == 2 ? "Organization" : "Theme", new {id = i.ItemId });
          }
    }

    public ActionResult Search(string SearchValue) {
      
      return View(im.SearchItems(SearchValue));
    }


  }
}