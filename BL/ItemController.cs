using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using System.Diagnostics;
using BL.Domain;

namespace BL
{
    public class ItemController
    {
        private List<SocialMediaPost> data = new List<SocialMediaPost>();
        private List<Item> items = new List<Item>();
        private List<Alert> alerts = new List<Alert>();

        private AlertManager alertManager = new AlertManager();
        public GebruikerManager gebruikerManager = new GebruikerManager(new User() { Username = "Theo_Franken", Password = "wachtwoord", Email = "theo.franken@nva.be" });
        private GebruikersManager gebruikersManager = new GebruikersManager();
        public NotificationManager notificationManager = new NotificationManager();
        private SocialMediaManager socialMediaManager = new SocialMediaManager();

        public ItemController()
        {
            notificationManager.AddNotification("Bart Dewever wordt positiever", gebruikerManager.GetUser(), "http://www.google.be");
            notificationManager.AddNotification("New tweet from Jan de Wever", new BL.Domain.User() { Id = 31, Email = "jordi@hotmail.com", Password = "strip", Username = "Bartje" }, "http://www.facebook.com");

        }

        public void SynchroniseDb()
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.bitzfactory.com/textgaindump.json"); //LINK VERANDEREN
            //request.Method = WebRequestMethods.Http.Get;
            //request.Accept = "application/json";
            //string text;
            //var response = (HttpWebResponse)request.GetResponse();

            //using (var sr = new StreamReader(response.GetResponseStream()))
            //{
            //    text = sr.ReadToEnd();
            //}
            
            foreach (SocialMediaPost post in data)
            {

            }
        }

        private void LeesData(DateTime startDate, DateTime endDate)
        {

        }
    }
}
