using BL.Domain;
using BL.Managers;
using politiekeBarometer.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace politiekeBarometer.Controllers
{
    public class HomeController : Controller
    {
        private ChartManager chartManager;
        public HomeController()
        {
            chartManager = new ChartManager();
        }

        public ActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.Charts = chartManager.GetStandardChart();
            foreach (var chart in viewModel.Charts)
            {
                chartManager.RetrieveDataChart(chart.Value);
            }
            return View(viewModel);
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}