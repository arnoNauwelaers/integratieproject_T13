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

namespace BL
{
    public class Read
    {
        private const string URL = "http://kdg.textgain.com/query";
        static HttpClient client = new HttpClient();
        public IEnumerable<SocialMediaPost> ReadData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.bitzfactory.com/textgain.json");
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            var tweets = JArray.Parse(text).ToObject<List<SocialMediaPost>>();
            return (List<SocialMediaPost>) tweets;
        }

        public IEnumerable<SocialMediaPost> ReadData2(string sinceDate)
        {
            var tweets = new List<SocialMediaPost>() ;
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
                    //TODO since met json nog catchen met postman
                    streamWriter.Write(json);
                }
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException("Something went wrong while reading the server response.")))
                {
                    tweets = JObject.Parse(streamReader.ReadToEnd()).ToObject<List<SocialMediaPost>>();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Er is een error opgetreden in de API reader: " + ex.ToString());
            }
            return tweets;
            
        }
    }
}
