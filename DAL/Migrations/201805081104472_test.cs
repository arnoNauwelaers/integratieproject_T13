namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApiFrequency = c.Int(nullable: false),
                        ApiUrl = c.String(),
                        ApiPort = c.String(),
                        DataLifetime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "AppToken", c => c.String());
            DropColumn("dbo.Platform", "Interval");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Platform", "Interval", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "AppToken");
            DropTable("dbo.Settings");
        }
    }
}
