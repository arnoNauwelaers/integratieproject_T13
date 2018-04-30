using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    //TODO per item een lijst met data kunnen bevatten
    public class Chart
    {
        [Key]
        public int ChartId { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ChartType ChartType { get; set; }
        public virtual ChartValue ChartValue { get; set; }
        public double X { get; set; } = 10;
        public double Y { get; set; } = 10;
        public double Height { get; set; } = 400;
        public double Width { get; set; } = 530;
        public Boolean Saved { get; set; } = false;
        public Boolean MultipleItems { get; set; } = false;
        [NotMapped]
        public virtual ICollection<ChartItemData> ChartItemData { get; set; } = new List<ChartItemData>();
        public virtual ICollection<ChartItemData> SavedChartItemData { get; set; } = new List<ChartItemData>();
        public DateFrequencyType FrequencyType { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;

        public string GetStyle()
        {
            return $"width: {Width}px; height: {Height}px; transform: translate({X}px, {Y}px);";
        }
    }

    //voor JSON deserializer
    public class TempChartEdit
    {
        public int Id;
        public double X;
        public double Y;
        public double Height;
        public double Width;
    }

    public class TempChartAdd
    {
        public string Items;
        public string ChartType;
        public string ChartValue;
        public string DateFrequency;
    }

    
}
