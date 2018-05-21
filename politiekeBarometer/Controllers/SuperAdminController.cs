using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BL.Domain;
using BL.Managers;
using DAL.EF;

namespace politiekeBarometer.Controllers
{
    public class SuperAdminController : Controller
    {
        private UnitOfWorkManager unitOfWorkManager;
        private SettingsManager settingsManager;

        public SuperAdminController()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            settingsManager = new SettingsManager(unitOfWorkManager);
        }
        

        // GET: Settings/Edit/
        public ActionResult EditSettings()
        {
            Settings settings = settingsManager.GetSettings();
            if (settings == null)
            {
                return HttpNotFound();
            }
            return View(settings);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSettings([Bind(Include = "Id,ApiFrequency,ApiUrl,ApiPort,DataLifetime")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                settingsManager.ChangeSettings(settings);
                return RedirectToAction("EditSettings");
            }
            return View(settings);
        }
    }
}
