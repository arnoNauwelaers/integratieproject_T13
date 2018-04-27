using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using DAL.EF;
using System.Web.Helpers;
using BL.Domain;

namespace politiekeBarometer.Controllers
{
  public class PageController : Controller
  {
    // GET: Page
    public ActionResult Person(int id) {
        ItemManager im = new ItemManager();
      Person p = im.ReadPerson(id);
      SocialMediaManager sm = new SocialMediaManager();
      List<Item> items = new List<Item>();
      items.Add(p);
      sm.GetDataFromPost(DateTime.Now.AddDays(-7), ChartValue.persons, items);
      //System.Web.Helpers.Chart testChart = new System.Web.Helpers.Chart(width: 600, height: 400).AddTitle("Test").AddSeries("Default",chartType: "Column", xValue:)
        return View(p);
        
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

    public ActionResult Search(string SearchValue) {
      ItemManager im = new ItemManager();
      return View(im.SearchItems(SearchValue));
    }


  }
}