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

namespace BL
{
    public class Read
    {
        private const string URL = "http://kdg.textgain.com/query";
        static HttpClient client = new HttpClient();
        public IEnumerable<SocialMediaPost> ReadData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.bitzfactory.com/json.php");
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            var tweets = JObject.Parse(text).SelectToken("records").ToObject<List<SocialMediaPost>>();
            return (List<SocialMediaPost>) tweets;
        }

        public IEnumerable<SocialMediaPost> ReadData2()
        {
            var tweets = new List<SocialMediaPost>() ;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            const string key = "X-API-Key";
            httpWebRequest.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException("Something went wrong while reading the server response.")))
                {
                    tweets = JObject.Parse(streamReader.ReadToEnd()).SelectToken("records").ToObject<List<SocialMediaPost>>();

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
