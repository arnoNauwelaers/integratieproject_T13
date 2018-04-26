using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Chart
    {
        [Key]
        public int ChartId { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ChartType ChartType { get; set; }
        public virtual ChartValue ChartValue { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public Boolean Saved { get; set; } = false;
        public virtual ICollection<Data> Data { get; set; } = new List<Data>();
        public DateFrequencyType FrequencyType { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }

    //voor JSON deserializer
    public class TempChart
    {
        public int Id;
        public int X;
        public int Y;
        public double Height;
        public double Width;
    }
}
