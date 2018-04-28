using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public class ChartItemData
    {
        [Key]
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<Data> Data { get; set; } = new List<Data>();
    }
}
