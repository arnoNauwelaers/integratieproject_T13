using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BL.Domain;

namespace Domain
{
    public class SocialMediaPost
    {
        [Key]
        [JsonProperty("id")]
        public int PostId { get; set; }
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

        [Required]
        public SocialMediaSource SocialMediaSource { get; set; }
        public ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; }
    }
}
