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

        public virtual ICollection<SocialMediaPost> SocialMediaPosts { get; set; } = new List<SocialMediaPost>();
        [Required]
        public virtual Item Item { get; set; }

        public SocialMediaProfile()
        {
        }
    }
}
