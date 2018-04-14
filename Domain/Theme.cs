using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        public ICollection<string> Keywords { get; set; }
    }
}