using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BL.Domain;
using BL.Managers;
using politiekeBarometer.Models;

namespace politiekeBarometer.Controllers
{
    //TODO YAGO: AUTHORIZATION DINGES
    public class PageController : Controller
    {
        ItemManager im;
        ApplicationUserManager userManager;
        AlertManager am;
        SocialMediaManager socialMediaManager;
        ChartManager chartManager;
        
        public PageController()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.im = new ItemManager(unitOfWorkManager);
            this.userManager = new ApplicationUserManager(unitOfWorkManager);
            this.am = new AlertManager(unitOfWorkManager);
            this.socialMediaManager = new SocialMediaManager(unitOfWorkManager);
            this.chartManager = new ChartManager(unitOfWorkManager);
        }

        public ActionResult Person(int id)
        {
            Person person = im.ReadPerson(id);
            ViewBag.Stories = socialMediaManager.GetTopTenUrlPerson(person);
            ViewBag.Charts = chartManager.GetChartsFromItem(person);
            ViewBag.RelatedWords = socialMediaManager.GetTopTenWordsPerson(person);
            return View(person);
        }

        public ActionResult Organization(int Id)
        {
            Organization organization = im.ReadOrganization(Id);
            ViewBag.Stories = socialMediaManager.GetTopTenUrlOrganization(organization);
            ViewBag.Charts = chartManager.GetChartsFromItem(organization);
            ViewBag.RelatedWords = socialMediaManager.GetTopTenWordsOrganization(organization);
            return View(organization);
        }

        public ActionResult Theme(int Id)
        {
            Theme theme = im.ReadTheme(Id);
            ViewBag.Charts = chartManager.GetChartsFromItem(theme);
            return View(theme);
        }

        public ActionResult OrganizationOverview()
        {
            List<Organization> orgs = (List<Organization>)im.GetOrganizations();
            ViewBag.Organizations = orgs;
            return View();

        }

        public ActionResult PersonOverview()
        {
            List<Person> pers = (List<Person>)im.GetPersons();
            ViewBag.Persons = pers;
            return View();
        }

        public ActionResult ThemeOverview()
        {
            List<Theme> themes = (List<Theme>)im.GetThemes();
            ViewBag.Themes = themes;
            return View();
        }

        public ActionResult AddItem(FormCollection collection)
        {
            ApplicationUser u = userManager.GetUser(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Item i = im.GetItem(Convert.ToInt32(collection["id"]));
            u.FollowedItems.Add(i);
            userManager.ChangeUser(u);
            if (i.GetType().ToString().Equals("Person")) { return RedirectToAction("Person", new { id = i.ItemId }); }
            else
            {
                return RedirectToAction(i.GetType().ToString().Equals("Organization") ? "Organization" : "Theme", new { id = i.ItemId });
            }
        }

        public ActionResult Search(string SearchValue)
        {
            return View(im.SearchItems(SearchValue));
        }

        public ActionResult CreateAlert()
        {
            var model = new AlertCreateViewModel();
            var itemSelect = im.GetItems().Select(x => new SelectListItem { Value = x.ItemId.ToString(), Text = x.Name });
            model.Items = new SelectList(itemSelect, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAlert(AlertCreateViewModel alert)
        {
            try
            {
                Alert a = new Alert
                {
                    Item = im.GetItem(alert.Id),
                    ConditionPerc = alert.Percentage
                    //User = userManager.g
                };
                return RedirectToAction("AlertOverview");
                // am.CreateAlert()
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetItems(string term = "")
        {
            var itemList = im.GetItems(term);
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
    }
}