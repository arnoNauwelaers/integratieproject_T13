﻿using System;
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
        public Boolean Saved { get; set; } = false;
        public virtual ICollection<Data> SavedData { get; set; } = new List<Data>();
        public virtual ICollection<Data> Data { get; set; } = new List<Data>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;
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
