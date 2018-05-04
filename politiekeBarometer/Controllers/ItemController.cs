using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Domain;
using BL;

namespace politiekeBarometer.Controllers
{
  public class ItemController : Controller
  {


    IItemManager itemManager = new ItemManager();
    SocialMediaManager socialMediaManager = new SocialMediaManager();
    // GET: Item
    public ActionResult Index()
    {


      return View();
    }

    public ActionResult OrganizationIndex()
    {
      List<Organization> organizations = itemManager.GetOrganizations();
      return View(organizations);
    }

    public ActionResult OrganizationDetails(int id)
    {
      Organization o = itemManager.ReadOrganization(id);
      ViewBag.Profiles = o.socialMediaProfiles;
      return View(o);

    }

    //GET: Item/DeleteOrganization
    public ActionResult DeleteOrganization(int id)
    {
      Organization o = itemManager.ReadOrganization(id);
      if (o.Equals(null))
      {
        return HttpNotFound();
      }
      return View(o);
    }
    // POST: Item/DeleteOrganizatin
    [HttpPost, ActionName("DeleteOrganization")]
    public ActionResult OrganizationDeleteConfirmed(int id)
    {

      try
      {
        Organization o = itemManager.ReadOrganization(id);
        itemManager.RemoveOrganization(o);

        return RedirectToAction("OrganizationIndex");

      }
      catch (Exception e)
      {
        Console.WriteLine(e.InnerException);
        return RedirectToAction("OrganizationIndex");
      }
    }

    // Get: Item/CreateOrganization
    public ActionResult CreateOrganization()
    {
      return View();
    }

    //Post: Item/CreateOrganization
    [HttpPost]
    public ActionResult CreateOrganization(FormCollection collection)
    {
      try
      {
        Organization o = new Organization
        {
          Name = collection["name"],
          typeInt = 2,
          socialMediaProfiles = new List<SocialMediaProfile>() { new SocialMediaProfile { Url = collection["url"], Source = collection["src"] } }
        };
        itemManager.AddOrganization(o);

        return RedirectToAction("OrganizationIndex");
      }
      catch (Exception e)
      {
        return View("Error " + e);
      }
    }

    public ActionResult EditOrganization(int id)
    {
      Organization o = itemManager.ReadOrganization(id);
      ViewBag.Organization = o;
      return View();
    }

    // POST: Item/EditOrganization
    [HttpPost]
    public ActionResult EditOrganization(FormCollection collection)
    {
      try
      {
        Organization o = itemManager.ReadOrganization(Convert.ToInt32(collection["id"]));
        o.Name = Convert.ToString(collection["name"]);

        itemManager.ChangeItem(o);

        return RedirectToAction("OrganizationIndex");
      }
      catch
      {
        return View();
      }
    }

    // GET: Item/CreatePerson
    public ActionResult CreatePerson()
    {

      ViewBag.Organizations = itemManager.GetOrganizations();

      return View();
    }
    [HttpGet]
    public ActionResult AddSocialmediaProfileToOrganization(int id)
    {
      ViewBag.Organization = itemManager.ReadOrganization(id);
      return View();

    }

    [HttpPost]
    public ActionResult AddSocialMediaProfileToOrganization(FormCollection collection)
    {
      try
      {
        Organization o = itemManager.ReadOrganization(Convert.ToInt32(collection["id"]));
        SocialMediaProfile smp = new SocialMediaProfile { Url = collection["url"], Source = collection["src"] };
        o.socialMediaProfiles.Add(smp);
        itemManager.ChangeItem(o);
        return RedirectToAction("OrganizationIndex");
      }
      catch (Exception e)
      {
        return View(e);
      }


    }

    // POST: Item/CreatePerson
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreatePerson(FormCollection collection)
    {

      ViewBag.Organizations = itemManager.GetOrganizations();

      try
      {
        Person p = new Person
        {
          Organization = itemManager.ReadOrganization(Convert.ToInt32(collection["organization"])),
          Name = collection["name"],
          SocialMediaProfiles = new List<SocialMediaProfile>() { new SocialMediaProfile { Url = collection["url"], Source = collection["src"] } },
          typeInt = 1
        };
        itemManager.AddPerson(p);

        return RedirectToAction("PersonIndex");

      }
      catch (Exception e)
      {
        return View("Error " + e);
      }
    }

    public ActionResult PersonIndex()
    {
      List<Person> persons = itemManager.GetPersons();
      return View(persons);
    }

    //GET: Item/DeletePerson
    public ActionResult DeletePerson(int id)
    {
      Person p = itemManager.ReadPerson(id);
      if (p.Equals(null))
      {
        return HttpNotFound();
      }
      return View(p);
    }
    // POST: Item/DeletePerson
    [HttpPost, ActionName("DeletePerson")]
    public ActionResult PersonDeleteConfirmed(int id)
    {

      try
      {
        Person p = itemManager.ReadPerson(id);
        itemManager.RemoveItem(p);

        return RedirectToAction("PersonIndex");

      }
      catch (Exception e)
      {
        Console.WriteLine(e.InnerException);
        return RedirectToAction("PersonIndex");
      }
    }

    public ActionResult EditPerson(int id)
    {
      Person p= itemManager.ReadPerson(id);
      ViewBag.Person = p;
      ViewBag.Organizations = itemManager.GetOrganizations();
      return View();
    }

    // POST: Item/EditPerson
    [HttpPost]
    public ActionResult EditPerson(FormCollection collection)
    {
      
      try
      {
        Person p = itemManager.ReadPerson(Convert.ToInt32(collection["id"]));
        p.Name = Convert.ToString(collection["name"]);
        p.Organization = itemManager.ReadOrganization(Convert.ToInt32(collection["organization"]));

        itemManager.ChangeItem(p);

        return RedirectToAction("PersonIndex");
      }
      catch
      {
        return View();
      }
    }

    public ActionResult PersonDetails(int id)
    {
      Person p = itemManager.ReadPerson(id);
      ViewBag.Profiles = p.SocialMediaProfiles;
      return View(p);

    }

    [HttpGet]
    public ActionResult AddSocialmediaProfileToPerson(int id)
    {
      ViewBag.Person = itemManager.ReadPerson(id);
      return View();

    }

    [HttpPost]
    public ActionResult AddSocialMediaProfileToPerson(FormCollection collection)
    {
      try
      {
        Person p = itemManager.ReadPerson(Convert.ToInt32(collection["id"]));
        SocialMediaProfile smp = new SocialMediaProfile { Url = collection["url"], Source = collection["src"] };
        p.SocialMediaProfiles.Add(smp);
        itemManager.ChangeItem(p);
        return RedirectToAction("PersonIndex");
      }
      catch (Exception e)
      {
        return View(e);
      }


    }


  }
}