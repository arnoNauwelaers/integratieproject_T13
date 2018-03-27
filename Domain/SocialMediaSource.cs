using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class SocialMediaSource
    {
        [Key]
        public int SourceId { get; set; }
        public String Url { get; set; }
        public string Ip { get; set; }

        public ICollection<Deelplatform_SocialMediaSource> Deelplatform_SocialMediaSource { get; set; }
        public ICollection<SocialMediaPost> SocialMediaPost { get; set; }
    }
}