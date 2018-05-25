using BL.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web.Script.Serialization;
using BL.Managers;

namespace BL
{
    public class Read
    {
        private string URL;
        static HttpClient client = new HttpClient();
        private SettingsManager settingsManager;
        public static DateTime? LastRead { get; set; } = null;

        public Read(UnitOfWorkManager unitOfWorkManager)
        {
            settingsManager = new SettingsManager(unitOfWorkManager);
        }

        public IEnumerable<SocialMediaPost> ReadData(string sinceDate)
        {
            try
            {
                var tweets = new List<SocialMediaPost>();
                Settings settings = settingsManager.GetSettings();
                URL = settings.ApiUrl;
                if (settings.ApiPort != null && !settings.ApiPort.Equals(""))
                {
                    URL += $":{settings.ApiPort}";
                }
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                if (sinceDate != null)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            since = sinceDate
                        });
                        streamWriter.Write(json);
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException("Something went wrong while reading the server response.")))
                {
                    tweets = JArray.Parse(streamReader.ReadToEnd()).ToObject<List<SocialMediaPost>>();
                }
                return tweets;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("API instellingen verkeerd: " + ex.Message);
            }
            return (new List<SocialMediaPost>());
        }
    }
}
