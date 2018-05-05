﻿using System;
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
        public string Url { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SocialMediaPost> SocialMediaPost { get; set; } = new List<SocialMediaPost>();
        /*
        public ICollection<Deelplatform_SocialMediaSource> Deelplatform_SocialMediaSource { get; set; }
        */
    }
}