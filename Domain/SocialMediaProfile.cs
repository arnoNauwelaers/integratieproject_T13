using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class SocialMediaProfile
    {
        [Key]
        public int ProfileId { get; set; }
        public String Url { get; set; }
        public String Source { get; set; }

        public List<SocialMediaPost> SocialMediaPosts { get; set; }
        [Required]
        public Item Item { get; set; }

        public SocialMediaProfile()
        {
            SocialMediaPosts = new List<SocialMediaPost>();
        }
    }
}
