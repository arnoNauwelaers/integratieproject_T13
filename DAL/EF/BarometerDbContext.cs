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
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.EF
{
    [DbConfigurationType(typeof(BarometerDbConfiguration))]
    internal class BarometerDbContext : DbContext /* 'public' for testing with project 'DAL-Testing'! */
    {
        public BarometerDbContext()
          : base("PolitiekeBarometerDB")
        {
            //Database.SetInitializer<SupportCenterDbContext>(new SupportCenterDbInitializer()); // moved to 'SupportCenterDbConfiguration'
        }


        public DbSet<Item> Items { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<SocialMediaPost> SocialMediaPosts { get; set; }
        public DbSet<SocialMediaProfile> SocialMediaProfiles { get; set; }
        public DbSet<SocialMediaSource> SocialMediaSources { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder); // does nothing! (empty body)

            // Remove pluralizing tablenames
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Remove cascading delete for all required-relationships
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // 'Ticket.TicketNumber' as unique identifier
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Notification>().HasKey(u => u.NotificationId);

            modelBuilder.Entity<Alert>().HasMany(i => i.Notifications).WithMany();
            modelBuilder.Entity<Item>().HasMany(i => i.Alerts).WithMany();
            modelBuilder.Entity<Organization>().HasMany(i => i.persons).WithMany();
            modelBuilder.Entity<Organization>().HasMany(i => i.socialMediaProfiles).WithMany();
            modelBuilder.Entity<Person>().HasMany(i => i.socialMediaProfiles).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.SocialMediaProfiles).WithMany();
            modelBuilder.Entity<SocialMediaProfile>().HasMany(i => i.SocialMediaPosts).WithMany();
            modelBuilder.Entity<SocialMediaSource>().HasMany(i => i.SocialMediaPost).WithMany();
            //modelBuilder.Entity<Theme>().HasMany(i => i.Keywords).WithMany();
            modelBuilder.Entity<User>().HasMany(i => i.Alerts).WithMany();

            //IDENTITY TABLES
            //TODO juiste database configureren
            //modelBuilder.Entity<IdentityUser>().ToTable("MyUsers").Property(p => p.Id).HasColumnName("UserId");
            //modelBuilder.Entity<User>().ToTable("MyUsers").Property(p => p.UseId).HasColumnName("UserId");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("MyUserRoles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("MyUserLogins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("MyUserClaims");
            //modelBuilder.Entity<IdentityRole>().ToTable("MyRoles");

        }
    }
}
