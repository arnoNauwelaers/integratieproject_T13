using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.EF;
using DAL;
using System.Diagnostics;

namespace DAL.EF
{
    internal class BarometerDbInitializer : DropCreateDatabaseIfModelChanges<BarometerDbContext>
    {
        public static List<ApplicationUser> users;
        public static List<SocialMediaProfile> SocialMediaProfiles;
        public static List<Item> items;
        public static List<Alert> alerts;
        public static List<Person> persons;
        public static List<Organization> organizations;

        protected override void Seed(BarometerDbContext context)
        {

        }
    }
}
