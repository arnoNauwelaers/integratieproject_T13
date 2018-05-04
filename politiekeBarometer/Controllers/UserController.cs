using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using BL;
using BL.Domain;

namespace politiekeBarometer.Controllers
{
    public class UserController : Controller
    {

    IAppUserManager userManager = new ApplicationUserManager();
        // GET: User
        public ActionResult Index()
        {
      List<ApplicationUser> users = userManager.GetUsers();
            return View(users);
        }

    public ActionResult Details(string id)
    {
      ApplicationUser au = userManager.GetUser(id);
      ViewBag.User = au;
      return View(au);
    }

    //GET: User/Delete
    public ActionResult Delete(string id)
    {
      ApplicationUser au = userManager.GetUser(id);
      
      if (au.Equals(null))
      {
        return HttpNotFound();
      }
      return View(au);
    }
    // POST: User/Delete
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(string id)
    {

      try
      {
        ApplicationUser au = userManager.GetUser(id);
        userManager.RemoveUser(au);

        return RedirectToAction("Index");

      }
      catch (Exception e)
      {
        Console.WriteLine(e.InnerException);
        return RedirectToAction("Index");
      }
    }

    public FileContentResult DownloadCSV()
    {
      List<ApplicationUser> users = userManager.GetUsers();
      var myExport = new CsvExport();
      foreach (ApplicationUser au in users)
      {
        
        myExport.AddRow();
        myExport["UserName"] = au.UserName;
        myExport["Password"] = au.PasswordHash;
        myExport["LastActivity"] = au.LastActivityDate;
        myExport["AccessFailedCount"] = au.AccessFailedCount;
        myExport["Email"] = au.Email;
        myExport["EmailConfirmed"] = au.EmailConfirmed;
        myExport["Facebook"] = au.Facebook;
        myExport["Google"] = au.Google;
        myExport["Lockoutenabled"] = au.LockoutEnabled;
        myExport["Lockout einddatum"] = au.LockoutEndDateUtc;
        myExport["telefoon"] = au.PhoneNumber;
        myExport["2 factor"] = au.TwoFactorEnabled;
        StringBuilder sb = new StringBuilder();
        foreach (Item i in au.followedItems)
        {
          sb.Append(i.Name);
          sb.Append(" ");
        }
        myExport["gevolgde paginas:"] = sb.ToString();
        
      }

      return File(myExport.ExportToBytes(), "text/csv", "users.csv");
    }
  }
}