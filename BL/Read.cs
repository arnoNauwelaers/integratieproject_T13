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
        private const string URL = "https://kdg.textgain.com/query";
        static HttpClient client = new HttpClient();

        //TODO elke .. minuten uitvoeren
        public IEnumerable<SocialMediaPost> ReadData(string sinceDate)
        {
            var tweets = new List<SocialMediaPost>();
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
                    tweets = JArray.Parse(streamReader.ReadToEnd()).ToObject<List<SocialMediaPost>>();
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
