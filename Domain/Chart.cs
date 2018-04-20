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
        public int X { get; set; }
        public int Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
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
