using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BL.Domain
{
    public class SocialMediaPost
    {
        [Key]
        [JsonProperty("id")]
        public long PostId { get; set; }
        [JsonProperty("geo")]
        public string Geo { get; set; }
        [JsonProperty("retweet")]
        public Boolean Retweet { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("sentiment")]
        public double[] sentiment { get; set; }
        [JsonProperty("hashtags")]
        public List<String> Hashtags { get; set; }
        [JsonProperty("urls")]
        public List<String> Verhalen { get; set; }
        [JsonProperty("words")]
        public List<String> Words { get; set; }
        [JsonProperty("politician")]
        public string[] Politician { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        public virtual SocialMediaSource SocialMediaSource { get; set; }
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();

    }
}
