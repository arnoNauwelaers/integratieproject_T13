using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Zone
    {
        [Key]
        public int Id { get; set; }
        public double X { get; set; } = 10;
        public double Y { get; set; } = 10;
        public double Height { get; set; } = 400;
        public double Width { get; set; } = 530;
    }
}