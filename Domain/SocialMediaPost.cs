using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BL.Domain
{
    public class SocialMediaPost
    {
        [Key]
        public long PostId { get; set; }
        [JsonProperty("id")]
        public string TweetId { get; set; }
        //TODO geo kan zowel array van double en boolean zijn
        //[JsonProperty("geo")]
        //public double[] Geo { get; set; }
        [JsonProperty("retweet")]
        public Boolean Retweet { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("sentiment")]
        public double[] Sentiment { get; set; }
        [JsonProperty("hashtags")]
        public string[] Hashtags { get; set; }
        [JsonProperty("urls")]
        public string[] Verhalen { get; set; }
        [JsonProperty("words")]
        public string[] Words { get; set; }
        [JsonProperty("persons")]
        public string[] Person { get; set; }
        [JsonProperty("themes")]
        public string[] Theme { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("profile")]
        public virtual SocialMediaProfile SocialMediaProfile { get; set; }
        public virtual SocialMediaSource SocialMediaSource { get; set; }
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();

    }
}
