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
using BL.Managers;

namespace politiekeBarometer.Controllers
{
    public class ItemController : Controller
    {
        ItemManager itemManager;
        SocialMediaManager socialMediaManager;

        public ItemController()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.itemManager = new ItemManager(unitOfWorkManager);
            this.socialMediaManager = new SocialMediaManager(unitOfWorkManager);
        }

        public ActionResult AdminItemIndex()
        {
            var model = new ItemViewModel
            {
                Persons = itemManager.GetPersons().ToList(),
                Organizations = itemManager.GetOrganizations().ToList(),
                Themes = itemManager.GetThemes().ToList(),
                FileError = null
            };
            return View(model);
        }

        public ActionResult AdminItemCreate()
        {
            var model = new ItemCreateViewModel();
            var organizationSelect = itemManager.GetOrganizations().Select(x => new SelectListItem { Value = x.ItemId.ToString(), Text = x.Name });
            model.Organizations = new SelectList(organizationSelect, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult AdminItemCreate(ItemCreateViewModel newItem)
        {
            try
            {
                itemManager.AddItem(newItem.Name, newItem.type, newItem.SelectedOrganizationId, newItem.StringKeywords, newItem.TwitterUrl);
                return RedirectToAction("AdminItemIndex");
            }
            catch
            {
                return View(newItem);
            }
        }

        public ActionResult AdminItemDelete(int ItemId)
        {
            try
            {
                itemManager.DeleteItem(ItemId);
                return RedirectToAction("AdminItemIndex");
            }
            catch
            {
                return RedirectToAction("AdminItemIndex");
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
            model.profileIds = new List<int>();

            if (item.GetType().ToString().Contains("Person"))
            {
                if (((Person)item).Organization != null)
                {
                    model.SelectedOrganizationId = ((Person)item).Organization.ItemId;
                }
                foreach (var profile in itemManager.GetProfiles(item))
                {
                    model.profileIds.Add(profile.Id);
                    model.TwitterUrl = profile.Url;
                }
                model.type = "Persoon";
            }
            else if (item.GetType().ToString().Contains("Organization"))
            {
                foreach (var profile in itemManager.GetProfiles(item))
                {
                    model.profileIds.Add(profile.Id);
                    model.TwitterUrl = profile.Url;
                }
                model.type = "Organisatie";
            }
            else if (item.GetType().ToString().Contains("Theme"))
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
            try
            {
                Item tempitem = itemManager.EditItem(ItemId, editItem.Name, editItem.type, editItem.SelectedOrganizationId, editItem.SelectedKeywords, editItem.StringKeywords);
                if (editItem.profileIds != null)
                {
                    itemManager.EditProfiles(editItem.profileIds, editItem.TwitterUrl);
                }
                else if(editItem.type != "Thema")
                {
                    itemManager.AddProfileToItem(tempitem, editItem.TwitterUrl);
                }

                if(editItem.TwitterUrl == null && editItem.profileIds != null)
                {
                    itemManager.DeleteProfiles(editItem.profileIds);
                }
                return RedirectToAction("AdminItemIndex");
            }
            catch
            {
                return View(editItem);
            }
            
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            var model = new ItemViewModel();
            if (upload != null && upload.ContentLength > 0)
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
                            itemManager.AddItem(itemString[0], itemString[1], itemString[2], itemString[3], itemString[4]);
                        }
                        catch
                        {
                            model.FileError = "(er is iets fout gegaan bij het aanmaken van de objecten. Mogelijk heb je het bestand fout geformateerd)";
                        }
                    }
                }
                else
                {
                    model.FileError = "(je hebt geen csv file geupload)";
                }

            }
            else
            {
                model.FileError = "(je hebt geen file geselecteerd of deze file is leeg)";
            }

            model.Persons = itemManager.GetPersons().ToList();
            model.Organizations = itemManager.GetOrganizations().ToList();
            model.Themes = itemManager.GetThemes().ToList();
            return View("AdminItemIndex", model);
        }
    }
}