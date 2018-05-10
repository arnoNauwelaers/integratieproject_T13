using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public int ApiFrequency { get; set; } //aantal minuten?
        public string ApiUrl { get; set; }
        public string ApiPort { get; set; }
        public int DataLifetime { get; set; } //aantal dagen?
        public List<Chart> standardCharts { get; set; } = new List<Chart>();
    }
}
