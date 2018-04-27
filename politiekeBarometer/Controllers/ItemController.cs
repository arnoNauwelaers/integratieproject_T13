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
      return View();
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
    public ActionResult DeleteConfirmed(int id)
    {

      try
      {
        Organization o = itemManager.ReadOrganization(id);
        itemManager.RemoveOrganization(o);
        
        return RedirectToAction("Index");

      }
      catch (Exception e)
      {
        Console.WriteLine(e.InnerException);
        return RedirectToAction("Index");
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
      catch(Exception e)
      {
        return View("Error " + e);
      }
    }

    // POST: Item/EditOrganization
    [HttpPost]
    public ActionResult Edit(FormCollection collection)
    {
      ViewBag.Profiles = socialMediaManager.GetSocialMediaProfiles();

      
      try
      {
        Organization o = itemManager.ReadOrganization(Convert.ToInt32(collection["id"]));
        o.socialMediaProfiles.Add(socialMediaManager.GetSocialMediaProfile(Convert.ToInt32(collection["smp"])));
        itemManager.ChangeItem(o);
        
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Item/CreatePerson
    public ActionResult CreatePerson()
    {



      return View();
    }

    // POST: Item/CreatePerson
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreatePerson(FormCollection collection)
    {

      try
      {
        Person p = new Person();
        p.Organization = itemManager.ReadOrganization(Convert.ToInt32(collection["organization"]));
        p.Name = collection["name"];
        p.socialMediaProfiles = new List<SocialMediaProfile>() { new SocialMediaProfile { Url = collection["url"], Source = collection["src"] } };
        p.typeInt = 1;
        itemManager.AddPerson(p);

        return RedirectToAction("Index");

      }
      catch(Exception e)
      {
        return View("Error " + e);
      }
    }

  }
}