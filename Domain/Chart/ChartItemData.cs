using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace BL.Domain
{
    public class ChartItemData
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; }
        public virtual ICollection<Data> Data { get; set; } = new List<Data>();
    }
}
