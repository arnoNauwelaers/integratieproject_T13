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
        public int ProfileId { get; set; }
        public int Name { get; set; }
        public String Url { get; set; }
        public String Source { get; set; }

        public ICollection<SocialMediaPost> SocialMediaPosts { get; set; }
        [Required]
        public Item Item { get; set; }
    }
}
