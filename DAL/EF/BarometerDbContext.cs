using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.Domain;

namespace DAL.EF
{
    [DbConfigurationType(typeof(DbConfiguration))]
    internal class BarometerDbContext : DbContext /* 'public' for testing with project 'DAL-Testing'! */
    {
        public BarometerDbContext()
          : base("PolitiekeBarometerDB")
        {
            //Database.SetInitializer<SupportCenterDbContext>(new SupportCenterDbInitializer()); // moved to 'SupportCenterDbConfiguration'
        }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Deelplatform_SocialMediaSource> Deelplatform_SocialMediaSources { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<SocialMediaPost> SocialMediaPosts { get; set; }
        public DbSet<SocialMediaProfile> SocialMediaProfiles { get; set; }
        public DbSet<SocialMediaSource> SocialMediaSources { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }


        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder); // does nothing! (empty body)

            // Remove pluralizing tablenames
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Remove cascading delete for all required-relationships
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // 'Ticket.TicketNumber' as unique identifier
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

       
        }
    }
}
