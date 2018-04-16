using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.EF;
using DAL;

namespace DAL.EF
{
    //TODO DropCreateDatabaseIf...
    internal class BarometerDbInitializer : DropCreateDatabaseIfModelChanges<BarometerDbContext>
    {
        protected override void Seed(BarometerDbContext context)
        {
            Memory.generateData();
            foreach (SocialMediaProfile profile in Memory.SocialMediaProfiles)
            {
                context.SocialMediaProfiles.Add(profile);
            }

            //foreach (ApplicationUser user in Memory.users)
            //{
            //    context.Users.Add(user);
            //}

            foreach (Person person in Memory.persons)
            {
                context.Persons.Add(person);
            }

            foreach (Organization organization in Memory.organizations)
            {
                context.Organizations.Add(organization);
            }

            foreach (Item item in Memory.items)
            {
                context.Items.Add(item);
            }

            foreach (Alert alert in Memory.alerts)
            {
                context.Alerts.Add(alert);
            }

            context.SaveChanges();
        }
    }
}
