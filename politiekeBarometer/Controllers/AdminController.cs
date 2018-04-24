using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using BL;
using BL.Domain;
using politiekeBarometer.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;


namespace politiekeBarometer.Controllers
{
  public class AdminController : Controller
  {
    private static ApplicationUserManager userManager = new ApplicationUserManager();
    // GET: Admin
    public ActionResult Index()
    {
      
        return View(userManager.GetUsersFromRole("Admin"));
      
    }

    // GET: Admin/Create
    public ActionResult Create()
    {



      return View();
    }

    // POST: Admin/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(RegisterViewModel model)
    {

      

      ApplicationUser user = new ApplicationUser
      {

        UserName = model.UserName,
        Email = model.Email,

      };
      var result = await userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        var result1 = await userManager.AddToRoleAsync(user.Id, "Admin");
        if (result1.Succeeded)
        {
          EmailSender.Instance.SendEmail(model.UserName, model.Email, model.Password);
        }
        return RedirectToAction("Index");
      }
      return View(model);
    }

    public ActionResult Delete(string id)
    {
      ApplicationUser user = userManager.GetUser(id);
      return View(user);
    }

    // POST: Party/Delete
    [HttpPost]
    public ActionResult Delete(string id, FormCollection collection)
    {
      try
      {

        userManager.DeleteAsync(userManager.FindById(id));

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Admin/Details
    public ActionResult Details(string id)
    {
      return View(userManager.GetUser(id));
    }

    // GET: Admin/Edit
    public ActionResult Edit(string id)
    {

      @ViewBag.Admin = userManager.GetUser(id);
      return View();
    }

    // POST: Admin/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(string id, FormCollection collection)
    {

      try
      {
        ApplicationUser user = userManager.GetUser(id);
        user.UserName = Convert.ToString(collection["username"]);
        user.Email = Convert.ToString(collection["email"]);
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

   
  }
  }