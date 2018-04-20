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
            string url = "http://kdg.textgain.com/query/";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
            client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("Accept", "application/json; charset=utf-8");
            try
            {
                var text = client.GetAsync(url);
                tweets = JObject.Parse(text.ToString()).SelectToken("records").ToObject<List<SocialMediaPost>>();
                foreach (var item in tweets)
                {
                    Debug.WriteLine(item.PostId + " blabla");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("blablabla");
            }
            return tweets;
        }
    }
}
