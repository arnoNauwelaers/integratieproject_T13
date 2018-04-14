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
    internal class DbInitializer : CreateDatabaseIfNotExists<BarometerDbContext>
    {
        protected override void Seed(BarometerDbContext context)
        {
            foreach (SocialMediaProfile profile in Memory.SocialMediaProfiles)
            {
                context.SocialMediaProfiles.Add(profile);
            }
            context.SaveChanges();
        }
    }
}
