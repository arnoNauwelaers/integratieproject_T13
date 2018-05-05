using System.Linq;
using System.Web.Mvc;
using BL.Domain;
using BL;
using politiekeBarometer.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;

namespace politiekeBarometer.Controllers
{
    public class ItemController : Controller
    {
        ItemManager itemManager = new ItemManager();
        SocialMediaManager socialMediaManager = new SocialMediaManager();

        public ActionResult AdminItemIndex()
        {
            var model = new ItemViewModel
            {
                Persons = itemManager.GetPersons().ToList(),
                organizations = itemManager.GetOrganizations().ToList(),
                themes = itemManager.GetThemes().ToList(),
                fileError = null
            };
            return View(model);
        }

        public ActionResult AdminItemCreate()
        {
            var model = new ItemCreateViewModel();
            var organizationSelect = itemManager.GetOrganizations().Select(x => new SelectListItem { Value = x.ItemId.ToString(), Text = x.Name});
            model.Organizations = new SelectList(organizationSelect, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult AdminItemCreate(ItemCreateViewModel newItem)
        {
            try
            {
                itemManager.AddItem(newItem.Name,newItem.type,newItem.SelectedOrganizationId,newItem.StringKeywords);
                return RedirectToAction("AdminItemIndex");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AdminItemDelete(int itemId)
        {
            try
            {
                itemManager.DeleteItem(itemId);
                return RedirectToAction("AdminItemIndex");
            }
            catch
            {
               return View();
            }
        }

        public ActionResult AdminItemEdit(int ItemId)
        {
            Item item = itemManager.ReadItem(ItemId);

            var model = new ItemCreateViewModel();
            var organizaionsSelect = itemManager.GetOrganizations().Select(x => new SelectListItem { Value = x.ItemId.ToString(), Text = x.Name });
            model.Organizations = new SelectList(organizaionsSelect, "Value", "Text");
            model.ItemId = item.ItemId;
            model.Name = item.Name;

            if (item.GetType().ToString().Contains("Person"))
            {
                if(((Person)item).Organization != null)
                {
                    model.SelectedOrganizationId = ((Person)item).Organization.ItemId;
                }
                model.type = "Persoon";
            }
            else if (item.GetType().ToString().Contains("Organization"))
            {
                model.type = "Organisatie";
            }else if (item.GetType().ToString().Contains("Theme"))
            {
                model.ListKeywords = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
                model.ListKeywords.AddRange((((Theme)item).Keywords).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Value }).ToList());
                model.type = "Thema";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AdminItemEdit(int ItemId, ItemCreateViewModel editItem)
        {
            itemManager.EditItem(ItemId, editItem.Name, editItem.type, editItem.SelectedOrganizationId, editItem.SelectedKeywords, editItem.StringKeywords);
            return RedirectToAction("AdminItemIndex");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            var model = new ItemViewModel();
            if (upload != null && upload.ContentLength > 0 )
            {
                if (upload.FileName.EndsWith(".csv"))
                {
                    StreamReader csvreader = new StreamReader(upload.InputStream);
                    string firstLine = csvreader.ReadLine();
                    string line;
                    while ((line = csvreader.ReadLine()) != null)
                    {
                        List<string> itemString = new List<string>();
                        try
                        {
                            itemString = (line.Split(';').ToList<string>());
                            itemManager.AddItem(itemString[0], itemString[1], itemString[2], itemString[3]);
                        }
                        catch
                        {
                            model.fileError = "(er is iets fout gegaan bij het aanmaken van de objecten. Mogelijk heb je het bestand fout geformateerd)";
                        }
                    }
                }
                else
                {
                    model.fileError = "(je hebt geen csv file geupload)";
                }
                
            }
            else
            {
                model.fileError = "(je hebt geen file geselecteerd of deze file is leeg)";
            }
            
            model.Persons = itemManager.GetPersons().ToList();
            model.organizations = itemManager.GetOrganizations().ToList();
            model.themes = itemManager.GetThemes().ToList();
            return View("AdminItemIndex",model);
        }

        //TODO delete
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: update logic
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: delete logic
                return RedirectToAction("Index");
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


        /*public ActionResult OrganizationIndex()
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return View("Error " + e);
            }
        }*/

    }
}