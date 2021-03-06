﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace BL.Domain
{
    public class Chart
    {
        [Key]
        public int ChartId { get; set; }
        public bool StandardChart { get; set; } = false; //standard zijn charts op bv. homepage
        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ChartType ChartType { get; set; }
        public virtual ChartValue ChartValue { get; set; }
        public virtual Zone Zone { get; set; }
        public Boolean Saved { get; set; } = false;
        public Boolean MultipleItems { get; set; } = false;
        public string ItemType { get; set; }
        public virtual ICollection<ChartItemData> ChartItemData { get; set; } = new List<ChartItemData>();
        public DateFrequencyType FrequencyType { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public List<string> Rgbas = new List<string>();
        //Nodig voor Colors
        [NotMapped]
        private static readonly Random rnd = new Random();
        [NotMapped]
        private List<string> Labels = new List<String>();
        [NotMapped]
        public DateTime? LastRead { get; set; }

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
            if (Saved == true && Rgbas.Count != 0)
            {
                return Rgbas;
            }
            Rgbas = new List<string>();
            if (ChartType == ChartType.pie || ChartType == ChartType.polarArea)
            {
                foreach (var item in ChartItemData)
                {
                    foreach (var data in item.Data)
                    {
                        Rgbas.Add(GenerateRandomRGBA());
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
                        Rgbas.Add(tempRgba);
                    }
                }
            }
            return Rgbas;
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
                DataSets.Add(new ChartDataSet(GetTitle(item.Item), GetRgbas(), 1, data));
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

        //overbodig nieuwe functie GetDataSets?
        public string GetData()
        {
            List<int> data = new List<int>();
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

        public string GetTitle(Item item)
        {
            string title = "";
            if (ChartValue == ChartValue.hashtags || ChartValue == ChartValue.words || ChartValue == ChartValue.persons || ChartValue == ChartValue.postsPerDate)
            {
                string word = "";
                switch (ChartValue)
                {
                    case ChartValue.postsPerDate: word = "posts"; break;
                    default: word = ChartValue.ToString(); break;
                }
                title = $"Aantal {word} van " + item.Name;
            }
            else if (ChartValue == ChartValue.trendMostNegative || ChartValue == ChartValue.trendMostPositive)
            {
                title = $"Meest ";
                if (ChartValue == ChartValue.trendMostNegative)
                {
                    title += "negatieve ";
                }
                else
                {
                    title += "positieve ";
                }
                title += ItemType + " (%)";
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
            if (Saved == true)
            {
                title += " (opgeslagen)";
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