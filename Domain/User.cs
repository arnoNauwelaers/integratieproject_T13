using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public bool Admin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public bool Geverifieerd { get; set; }
        public bool Google { get; set; }
        public bool Facebook { get; set; }
        public int AantalAanmeldingen { get; set; }
        public int TijdActief { get; set; }

        /*[Required]
        public ICollection<Deelplatform> Deelplatformen { get; set; }*/
        /*[Required]
        public ICollection<Dashboard> Dashboards { get; set; }*/
        public ICollection<Alert> Alerts { get; set; }
    }
}
