using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Theme : Item
    {
        public ICollection<string> Keywords { get; set; } = new List<string>();

      public Theme() {
        typeInt = 3;
      }
    }
}