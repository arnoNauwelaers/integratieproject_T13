using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BL.Domain
{
    //TODO per item een lijst met data kunnen bevatten
    public class Chart
    {
        [Key]
        public int ChartId { get; set; }
        public Boolean Standard { get; set; } = false; //standard zijn charts op bv. homepage
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ChartType ChartType { get; set; }
        public virtual ChartValue ChartValue { get; set; }
        public virtual Zone Zone { get; set; }
        public Boolean Saved { get; set; } = false;
        public Boolean MultipleItems { get; set; } = false;
        [NotMapped]
        public virtual ICollection<ChartItemData> ChartItemData { get; set; } = new List<ChartItemData>();
        public virtual ICollection<ChartItemData> SavedChartItemData { get; set; } = new List<ChartItemData>();
        public DateFrequencyType FrequencyType { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        //Nodig voor Colors
        [NotMapped]
        private static readonly Random rnd = new Random();
        [NotMapped]
        private List<string> Labels = new List<String>();

        public string GetStyle()
        {
            return $"transform: translate({Zone.X}px, {Zone.Y}px);";
        }

        public string GetItemNames()
        {
            string itemNames = "";
            if (Items.Count > 0)
            {
                foreach (var item in Items)
                {
                    itemNames += item.Name;
                    if (item != Items.Last())
                    {
                        itemNames += ",";
                    }
                }
            }
            return itemNames;
        }

        public List<string> GetRgbas()
        {
            List<string> rgbas = new List<string>();
            if (ChartType == ChartType.pie || ChartType == ChartType.polarArea)
            {
                foreach (var item in ChartItemData)
                {
                    foreach (var data in item.Data)
                    {
                        rgbas.Add(GenerateRandomRGBA());
                    }
                }
            }
            else if (ChartType == ChartType.bar || ChartType == ChartType.radar || ChartType == ChartType.line)
            {
                foreach (var item in ChartItemData)
                {
                    string tempRgba = GenerateRandomRGBA();
                    foreach (var data in item.Data)
                    {
                        rgbas.Add(tempRgba);
                    }
                }
            }
            return rgbas;
        }

        public string GetDataSets()
        {
            List<ChartDataSet> DataSets = new List<ChartDataSet>();
            foreach (var item in ChartItemData)
            {
                List<int> data = new List<int>();
                foreach (var label in Labels)
                {
                    Boolean found = false;
                    foreach (var itemData in item.Data)
                    {
                        if (label.Equals(itemData.Name))
                        {
                            data.Add(itemData.Amount);
                            found = true;
                            continue;
                        }
                    }
                    if (found == false)
                    {
                        data.Add(0);
                    }
                }
                DataSets.Add(new ChartDataSet(GetTitle(), GetRgbas(), 1, data));
            }
            return JsonConvert.SerializeObject(DataSets);
        }

        public string GenerateRandomRGBA()
        {
            int r = rnd.Next(0, 256);
            int g = rnd.Next(0, 256);
            int b = rnd.Next(0, 256);
            string opacity = "0.6";
            return $"rgba({r}, {g}, {b}, {opacity})";
        }

        public string GetWidth()
        {
            return Zone.Width.ToString().Replace(',', '.');
        }

        public string GetCanvasId()
        {
            return $"canvas{ChartId}";
        }

        public string GetChartName()
        {
            return $"chart{ChartId}";
        }

        public string GetDivId()
        {
            return $"div{ChartId}";
        }

        public string GetLabels()
        {
            Labels = new List<string>();
            foreach (var chartItemData in ChartItemData)
            {
                foreach (var item in chartItemData.Data)
                {
                    if (!Labels.Contains(item.Name))
                    {
                        Labels.Add(item.Name);
                    }
                }
            }
            return JsonConvert.SerializeObject(Labels);
        }

        public string GetTitle()
        {
            string title = "";
            if (ChartValue == ChartValue.hashtags || ChartValue == ChartValue.words || ChartValue == ChartValue.persons)
            {
                title = $"Aantal {ChartValue} van ";
                int i = 1;
                foreach (var item in ChartItemData)
                {
                    if (i == 1)
                    {
                        title += item.Item.Name;
                    }
                    else
                    {
                        title += $" & {item.Item.Name}";
                    }
                    i++;
                }
            }
            else
            {
                title = $"Aantal posts per ";
                if (ChartValue == ChartValue.trendPersons)
                {
                    title += "persoon";
                }
                else if (ChartValue == ChartValue.trendOrganizations)
                {
                    title += "organisatie";
                }
                else if (ChartValue == ChartValue.trendThemes)
                {
                    title += "thema";
                }
            }
            return title;
        }
    }

    //voor JSON deserializer
    public class TempChartEdit
    {
        public int Id;
        public double X;
        public double Y;
        public double Width;
    }

    public class TempChartAdd
    {
        public string Items;
        public string ChartType;
        public string ChartValue;
        public string DateFrequency;
    }

    public class ChartDataSet
    {
        public string label;
        public List<string> backgroundColor;
        public int borderWidth;
        public List<int> data;

        public ChartDataSet(string label, List<string> backgroundColor, int borderWidth, List<int> data)
        {
            this.label = label;
            this.backgroundColor = backgroundColor;
            this.borderWidth = borderWidth;
            this.data = data;
        }
    }
}