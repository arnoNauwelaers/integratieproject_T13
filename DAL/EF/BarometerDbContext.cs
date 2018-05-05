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
    public class BarometerDbContext : IdentityDbContext<ApplicationUser> /* 'public' for testing with project 'DAL-Testing'! */
    {
        

        public BarometerDbContext() : base("PolitiekeBarometerDB")
        {
            //Database.SetInitializer<SupportCenterDbContext>(new SupportCenterDbInitializer()); // moved to 'SupportCenterDbConfiguration'
        }

        public static BarometerDbContext Create()
    {
      return new BarometerDbContext();
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
        public DbSet<Chart> Charts { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Word> Woorden { get; set; }
        public DbSet<Sentiment> Sentiments { get; set; }
        public DbSet<ChartItemData> ChartItemDatas { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        //public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // needed
      // identity
      modelBuilder.Entity<ApplicationUser>().ToTable("Users");
      modelBuilder.Entity<IdentityRole>().ToTable("Roles");
      modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
      modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
      modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

      

      // Remove pluralizing tablenames
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Remove cascading delete for all required-relationships
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            
            //modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
            modelBuilder.Entity<Notification>().HasKey(u => u.NotificationId);

            // modelBuilder.Entity<IdentityUserLogin>().HasKey(u => u.UserId);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(u => new (u.RoleId, uint.))

            modelBuilder.Entity<Alert>().HasMany(i => i.Notifications).WithMany();
            modelBuilder.Entity<Chart>().HasMany(i => i.SavedChartItemData).WithMany();
            modelBuilder.Entity<Item>().HasMany(i => i.Alerts).WithMany();
            modelBuilder.Entity<Organization>().HasMany(i => i.persons).WithMany();
            modelBuilder.Entity<Organization>().HasMany(i => i.socialMediaProfiles).WithMany();
            modelBuilder.Entity<Person>().HasMany(i => i.SocialMediaProfiles).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.SocialMediaProfiles).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Words).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Urls).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Persons).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Hashtags).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Sentiments).WithMany();
            modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Themes).WithMany();
            //modelBuilder.Entity<Theme>().HasMany(i => i.Keywords).WithMany();
            modelBuilder.Entity<Keyword>().HasOptional(k => k.Theme);
            modelBuilder.Entity<ChartItemData>().HasMany(i => i.Data).WithMany();
            //modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Words).WithMany();
            //modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Person).WithMany();
            //modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Verhalen).WithMany();
            //modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Hashtags).WithMany();
            //TODO fixen: modelBuilder.Entity<SocialMediaPost>().HasMany(i => i.Sentiment).WithMany();
            modelBuilder.Entity<SocialMediaProfile>().HasMany(i => i.SocialMediaPosts).WithMany();
            modelBuilder.Entity<SocialMediaSource>().HasMany(i => i.SocialMediaPost).WithMany();
            modelBuilder.Entity<Chart>().HasMany(i => i.Items).WithMany();
            modelBuilder.Entity<Platform>().HasMany(i => i.Users).WithMany();
            modelBuilder.Entity<Platform>().HasMany(i => i.Sources).WithMany();
            //modelBuilder.Entity<Theme>().HasMany(i => i.Keywords).WithMany();
            //modelBuilder.Entity<User>().HasMany(i => i.Alerts).WithMany();


        }


        

        //public System.Data.Entity.DbSet<BL.Domain.ApplicationUser> ApplicationUsers { get; set; }
    }
}
