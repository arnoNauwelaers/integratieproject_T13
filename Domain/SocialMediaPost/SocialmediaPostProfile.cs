using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class SocialMediaPostProfile
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("age")]
        public string Age { get; set; }
        [JsonProperty("education")]
        public string Education { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("personality")]
        public string Personality { get; set; }
        public virtual SocialMediaPost SocialMediaPosts { get; set; }

        public SocialMediaPostProfile()
        {
        }
    }
}
