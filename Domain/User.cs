using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(25, MinimumLength = 3)]
        public string Username { get; set; }
        [StringLength(25, MinimumLength = 6)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Alert> Alerts { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            User u = (User)obj;
            return this.Id==u.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -2019361818;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Alert>>.Default.GetHashCode(Alerts);
            return hashCode;
        }
    }
}
