using DAL;
using Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SocialMediaManager
    {
        private SocialMediaRepository SocialMediaRepository;
        private ItemManager ItemManager;

        public SocialMediaManager()
        {
            SocialMediaRepository = new SocialMediaRepository();
            ItemManager = new ItemManager();
        }

        public void ReadData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.bitzfactory.com/textgaindump.json");
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            var tweets = JObject.Parse(text).SelectToken("records").ToObject<IEnumerable<SocialMediaPost>>(); ;
        }

    }
}
