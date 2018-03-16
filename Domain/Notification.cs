using BL.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; } //inhoud van bericht
        public User User { get; set; } //gebruiker die alert ontvangt
        public string Location { get; set; } //relatieve locatie pagina
    }
}
