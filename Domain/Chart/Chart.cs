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
        public virtual Zone Zone { get; set; } = new Zone();
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
            return $"width: {Zone.Width}px; height: {Zone.Height}px; transform: translate({Zone.X}px, {Zone.Y}px);";
        }

        public string GetCanvasId()
        {
            return $"canvas{ChartId}";
        }

        public string GetLabels()
        {
            string labels = "";
            foreach (var chartItemData in ChartItemData)
            {
                foreach (var item in chartItemData.Data)
                {
                    labels += "\"" + item.Name + "\",";
                }
            }
            return labels;
        }
        public string GetData()
        {
            string data = "";
            foreach (var chartItemData in ChartItemData)
            {
                foreach (var item in chartItemData.Data)
                {
                    data += item.Amount + ", ";
                }
            }
            return data;
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
