using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class SocialMediaPost
    {
        [Key]
        public int PostId { get; set; }
        public string Geo { get; set; }
        public Boolean Retweet { get; set; }
        public DateTime Date { get; set; }
        public int Polarity { get; set; }
        public int Objectivity { get; set; }
        public List<String> Hashtags { get; set; }
        public List<String> Verhalen { get; set; }
        public List<String> Words { get; set; }

        [Required]
        public SocialMediaSource SocialMediaSource { get; set; }
        public ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; }
    }
}
