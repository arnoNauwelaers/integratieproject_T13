namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alert",
                c => new
                    {
                        AlertId = c.Int(nullable: false, identity: true),
                        Parameter = c.Byte(nullable: false),
                        ConditionPerc = c.Double(nullable: false),
                        CompareItem_ItemId = c.Int(),
                        Item_ItemId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AlertId)
                .ForeignKey("dbo.Item", t => t.CompareItem_ItemId)
                .ForeignKey("dbo.Item", t => t.Item_ItemId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.CompareItem_ItemId)
                .Index(t => t.Item_ItemId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Organization_ItemId = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Item", t => t.Organization_ItemId)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .Index(t => t.Organization_ItemId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SocialMediaProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Source = c.String(),
                        Gender = c.String(),
                        Age = c.String(),
                        Education = c.String(),
                        Language = c.String(),
                        Personality = c.String(),
                        Item_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.Item_ItemId)
                .Index(t => t.Item_ItemId);
            
            CreateTable(
                "dbo.SocialMediaPost",
                c => new
                    {
                        PostId = c.Long(nullable: false, identity: true),
                        TweetId = c.String(),
                        Retweet = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Source = c.String(),
                        PostSentiment_Id = c.Int(),
                        SocialMediaProfile_Id = c.Int(),
                        SocialMediaSource_SourceId = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Sentiment", t => t.PostSentiment_Id, cascadeDelete: true)
                .ForeignKey("dbo.SocialMediaProfile", t => t.SocialMediaProfile_Id)
                .ForeignKey("dbo.SocialMediaSource", t => t.SocialMediaSource_SourceId)
                .Index(t => t.PostSentiment_Id)
                .Index(t => t.SocialMediaProfile_Id)
                .Index(t => t.SocialMediaSource_SourceId);
            
            CreateTable(
                "dbo.Hashtag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sentiment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Polarity = c.Double(nullable: false),
                        Subjectivity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocialMediaSource",
                c => new
                    {
                        SourceId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Ip = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SourceId);
            
            CreateTable(
                "dbo.Keyword",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Theme_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.Theme_ItemId)
                .Index(t => t.Theme_ItemId);
            
            CreateTable(
                "dbo.Url",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Word",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Content = c.String(),
                        Alert_AlertId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Alert", t => t.Alert_AlertId)
                .Index(t => t.Alert_AlertId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Geverifieerd = c.Boolean(nullable: false),
                        Google = c.Boolean(nullable: false),
                        Facebook = c.Boolean(nullable: false),
                        AantalAanmeldingen = c.Int(nullable: false),
                        LastActivityDate = c.DateTime(),
                        TijdActief = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Platform_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Platform_Id);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Chart",
                c => new
                    {
                        ChartId = c.Int(nullable: false, identity: true),
                        ChartType = c.Byte(nullable: false),
                        ChartValue = c.Int(nullable: false),
                        Saved = c.Boolean(nullable: false),
                        MultipleItems = c.Boolean(nullable: false),
                        FrequencyType = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Zone_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ChartId)
                .ForeignKey("dbo.Zone", t => t.Zone_Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .Index(t => t.Zone_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ChartItemData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.Item_ItemId)
                .Index(t => t.Item_ItemId);
            
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Platform",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Interval = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ItemAlert",
                c => new
                    {
                        Item_ItemId = c.Int(nullable: false),
                        Alert_AlertId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_ItemId, t.Alert_AlertId })
                .ForeignKey("dbo.Item", t => t.Item_ItemId)
                .ForeignKey("dbo.Alert", t => t.Alert_AlertId)
                .Index(t => t.Item_ItemId)
                .Index(t => t.Alert_AlertId);
            
            CreateTable(
                "dbo.SocialMediaPostHashtag",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        Hashtag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.Hashtag_Id })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.Hashtag", t => t.Hashtag_Id)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.Hashtag_Id);
            
            CreateTable(
                "dbo.SocialMediaPostPerson",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        Person_ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.Person_ItemId })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.Item", t => t.Person_ItemId)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.Person_ItemId);
            
            CreateTable(
                "dbo.SocialMediaPostSocialMediaProfile",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        SocialMediaProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.SocialMediaProfile_Id })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.SocialMediaProfile", t => t.SocialMediaProfile_Id)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.SocialMediaProfile_Id);
            
            CreateTable(
                "dbo.SocialMediaSourceSocialMediaPost",
                c => new
                    {
                        SocialMediaSource_SourceId = c.Int(nullable: false),
                        SocialMediaPost_PostId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaSource_SourceId, t.SocialMediaPost_PostId })
                .ForeignKey("dbo.SocialMediaSource", t => t.SocialMediaSource_SourceId)
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .Index(t => t.SocialMediaSource_SourceId)
                .Index(t => t.SocialMediaPost_PostId);
            
            CreateTable(
                "dbo.SocialMediaPostTheme",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        Theme_ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.Theme_ItemId })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.Item", t => t.Theme_ItemId)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.Theme_ItemId);
            
            CreateTable(
                "dbo.SocialMediaPostUrl",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        Url_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.Url_Id })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.Url", t => t.Url_Id)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.Url_Id);
            
            CreateTable(
                "dbo.SocialMediaPostWord",
                c => new
                    {
                        SocialMediaPost_PostId = c.Long(nullable: false),
                        Word_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaPost_PostId, t.Word_Id })
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .ForeignKey("dbo.Word", t => t.Word_Id)
                .Index(t => t.SocialMediaPost_PostId)
                .Index(t => t.Word_Id);
            
            CreateTable(
                "dbo.SocialMediaProfileSocialMediaPost",
                c => new
                    {
                        SocialMediaProfile_Id = c.Int(nullable: false),
                        SocialMediaPost_PostId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocialMediaProfile_Id, t.SocialMediaPost_PostId })
                .ForeignKey("dbo.SocialMediaProfile", t => t.SocialMediaProfile_Id)
                .ForeignKey("dbo.SocialMediaPost", t => t.SocialMediaPost_PostId)
                .Index(t => t.SocialMediaProfile_Id)
                .Index(t => t.SocialMediaPost_PostId);
            
            CreateTable(
                "dbo.PersonSocialMediaProfile",
                c => new
                    {
                        Person_ItemId = c.Int(nullable: false),
                        SocialMediaProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_ItemId, t.SocialMediaProfile_Id })
                .ForeignKey("dbo.Item", t => t.Person_ItemId)
                .ForeignKey("dbo.SocialMediaProfile", t => t.SocialMediaProfile_Id)
                .Index(t => t.Person_ItemId)
                .Index(t => t.SocialMediaProfile_Id);
            
            CreateTable(
                "dbo.OrganizationPerson",
                c => new
                    {
                        Organization_ItemId = c.Int(nullable: false),
                        Person_ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_ItemId, t.Person_ItemId })
                .ForeignKey("dbo.Item", t => t.Organization_ItemId)
                .ForeignKey("dbo.Item", t => t.Person_ItemId)
                .Index(t => t.Organization_ItemId)
                .Index(t => t.Person_ItemId);
            
            CreateTable(
                "dbo.OrganizationSocialMediaProfile",
                c => new
                    {
                        Organization_ItemId = c.Int(nullable: false),
                        SocialMediaProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_ItemId, t.SocialMediaProfile_Id })
                .ForeignKey("dbo.Item", t => t.Organization_ItemId)
                .ForeignKey("dbo.SocialMediaProfile", t => t.SocialMediaProfile_Id)
                .Index(t => t.Organization_ItemId)
                .Index(t => t.SocialMediaProfile_Id);
            
            CreateTable(
                "dbo.AlertNotification",
                c => new
                    {
                        Alert_AlertId = c.Int(nullable: false),
                        Notification_NotificationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Alert_AlertId, t.Notification_NotificationId })
                .ForeignKey("dbo.Alert", t => t.Alert_AlertId)
                .ForeignKey("dbo.Notification", t => t.Notification_NotificationId)
                .Index(t => t.Alert_AlertId)
                .Index(t => t.Notification_NotificationId);
            
            CreateTable(
                "dbo.ChartItem",
                c => new
                    {
                        Chart_ChartId = c.Int(nullable: false),
                        Item_ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chart_ChartId, t.Item_ItemId })
                .ForeignKey("dbo.Chart", t => t.Chart_ChartId)
                .ForeignKey("dbo.Item", t => t.Item_ItemId)
                .Index(t => t.Chart_ChartId)
                .Index(t => t.Item_ItemId);
            
            CreateTable(
                "dbo.ChartItemDataData",
                c => new
                    {
                        ChartItemData_Id = c.Int(nullable: false),
                        Data_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChartItemData_Id, t.Data_Id })
                .ForeignKey("dbo.ChartItemData", t => t.ChartItemData_Id)
                .ForeignKey("dbo.Data", t => t.Data_Id)
                .Index(t => t.ChartItemData_Id)
                .Index(t => t.Data_Id);
            
            CreateTable(
                "dbo.ChartChartItemData",
                c => new
                    {
                        Chart_ChartId = c.Int(nullable: false),
                        ChartItemData_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chart_ChartId, t.ChartItemData_Id })
                .ForeignKey("dbo.Chart", t => t.Chart_ChartId)
                .ForeignKey("dbo.ChartItemData", t => t.ChartItemData_Id)
                .Index(t => t.Chart_ChartId)
                .Index(t => t.ChartItemData_Id);
            
            CreateTable(
                "dbo.PlatformSocialMediaSource",
                c => new
                    {
                        Platform_Id = c.Int(nullable: false),
                        SocialMediaSource_SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Platform_Id, t.SocialMediaSource_SourceId })
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .ForeignKey("dbo.SocialMediaSource", t => t.SocialMediaSource_SourceId)
                .Index(t => t.Platform_Id)
                .Index(t => t.SocialMediaSource_SourceId);
            
            CreateTable(
                "dbo.PlatformApplicationUser",
                c => new
                    {
                        Platform_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Platform_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Platform", t => t.Platform_Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .Index(t => t.Platform_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PlatformApplicationUser", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.PlatformApplicationUser", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.PlatformSocialMediaSource", "SocialMediaSource_SourceId", "dbo.SocialMediaSource");
            DropForeignKey("dbo.PlatformSocialMediaSource", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.Users", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Item", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.Chart", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.Chart", "Zone_Id", "dbo.Zone");
            DropForeignKey("dbo.ChartChartItemData", "ChartItemData_Id", "dbo.ChartItemData");
            DropForeignKey("dbo.ChartChartItemData", "Chart_ChartId", "dbo.Chart");
            DropForeignKey("dbo.ChartItemData", "Item_ItemId", "dbo.Item");
            DropForeignKey("dbo.ChartItemDataData", "Data_Id", "dbo.Data");
            DropForeignKey("dbo.ChartItemDataData", "ChartItemData_Id", "dbo.ChartItemData");
            DropForeignKey("dbo.ChartItem", "Item_ItemId", "dbo.Item");
            DropForeignKey("dbo.ChartItem", "Chart_ChartId", "dbo.Chart");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Alert", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AlertNotification", "Notification_NotificationId", "dbo.Notification");
            DropForeignKey("dbo.AlertNotification", "Alert_AlertId", "dbo.Alert");
            DropForeignKey("dbo.Notification", "Alert_AlertId", "dbo.Alert");
            DropForeignKey("dbo.Alert", "Item_ItemId", "dbo.Item");
            DropForeignKey("dbo.Alert", "CompareItem_ItemId", "dbo.Item");
            DropForeignKey("dbo.OrganizationSocialMediaProfile", "SocialMediaProfile_Id", "dbo.SocialMediaProfile");
            DropForeignKey("dbo.OrganizationSocialMediaProfile", "Organization_ItemId", "dbo.Item");
            DropForeignKey("dbo.OrganizationPerson", "Person_ItemId", "dbo.Item");
            DropForeignKey("dbo.OrganizationPerson", "Organization_ItemId", "dbo.Item");
            DropForeignKey("dbo.PersonSocialMediaProfile", "SocialMediaProfile_Id", "dbo.SocialMediaProfile");
            DropForeignKey("dbo.PersonSocialMediaProfile", "Person_ItemId", "dbo.Item");
            DropForeignKey("dbo.SocialMediaProfileSocialMediaPost", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaProfileSocialMediaPost", "SocialMediaProfile_Id", "dbo.SocialMediaProfile");
            DropForeignKey("dbo.SocialMediaPostWord", "Word_Id", "dbo.Word");
            DropForeignKey("dbo.SocialMediaPostWord", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaPostUrl", "Url_Id", "dbo.Url");
            DropForeignKey("dbo.SocialMediaPostUrl", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaPostTheme", "Theme_ItemId", "dbo.Item");
            DropForeignKey("dbo.SocialMediaPostTheme", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.Keyword", "Theme_ItemId", "dbo.Item");
            DropForeignKey("dbo.SocialMediaPost", "SocialMediaSource_SourceId", "dbo.SocialMediaSource");
            DropForeignKey("dbo.SocialMediaSourceSocialMediaPost", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaSourceSocialMediaPost", "SocialMediaSource_SourceId", "dbo.SocialMediaSource");
            DropForeignKey("dbo.SocialMediaPostSocialMediaProfile", "SocialMediaProfile_Id", "dbo.SocialMediaProfile");
            DropForeignKey("dbo.SocialMediaPostSocialMediaProfile", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaPost", "SocialMediaProfile_Id", "dbo.SocialMediaProfile");
            DropForeignKey("dbo.SocialMediaPost", "PostSentiment_Id", "dbo.Sentiment");
            DropForeignKey("dbo.SocialMediaPostPerson", "Person_ItemId", "dbo.Item");
            DropForeignKey("dbo.SocialMediaPostPerson", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaPostHashtag", "Hashtag_Id", "dbo.Hashtag");
            DropForeignKey("dbo.SocialMediaPostHashtag", "SocialMediaPost_PostId", "dbo.SocialMediaPost");
            DropForeignKey("dbo.SocialMediaProfile", "Item_ItemId", "dbo.Item");
            DropForeignKey("dbo.Item", "Organization_ItemId", "dbo.Item");
            DropForeignKey("dbo.ItemAlert", "Alert_AlertId", "dbo.Alert");
            DropForeignKey("dbo.ItemAlert", "Item_ItemId", "dbo.Item");
            DropIndex("dbo.PlatformApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PlatformApplicationUser", new[] { "Platform_Id" });
            DropIndex("dbo.PlatformSocialMediaSource", new[] { "SocialMediaSource_SourceId" });
            DropIndex("dbo.PlatformSocialMediaSource", new[] { "Platform_Id" });
            DropIndex("dbo.ChartChartItemData", new[] { "ChartItemData_Id" });
            DropIndex("dbo.ChartChartItemData", new[] { "Chart_ChartId" });
            DropIndex("dbo.ChartItemDataData", new[] { "Data_Id" });
            DropIndex("dbo.ChartItemDataData", new[] { "ChartItemData_Id" });
            DropIndex("dbo.ChartItem", new[] { "Item_ItemId" });
            DropIndex("dbo.ChartItem", new[] { "Chart_ChartId" });
            DropIndex("dbo.AlertNotification", new[] { "Notification_NotificationId" });
            DropIndex("dbo.AlertNotification", new[] { "Alert_AlertId" });
            DropIndex("dbo.OrganizationSocialMediaProfile", new[] { "SocialMediaProfile_Id" });
            DropIndex("dbo.OrganizationSocialMediaProfile", new[] { "Organization_ItemId" });
            DropIndex("dbo.OrganizationPerson", new[] { "Person_ItemId" });
            DropIndex("dbo.OrganizationPerson", new[] { "Organization_ItemId" });
            DropIndex("dbo.PersonSocialMediaProfile", new[] { "SocialMediaProfile_Id" });
            DropIndex("dbo.PersonSocialMediaProfile", new[] { "Person_ItemId" });
            DropIndex("dbo.SocialMediaProfileSocialMediaPost", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaProfileSocialMediaPost", new[] { "SocialMediaProfile_Id" });
            DropIndex("dbo.SocialMediaPostWord", new[] { "Word_Id" });
            DropIndex("dbo.SocialMediaPostWord", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaPostUrl", new[] { "Url_Id" });
            DropIndex("dbo.SocialMediaPostUrl", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaPostTheme", new[] { "Theme_ItemId" });
            DropIndex("dbo.SocialMediaPostTheme", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaSourceSocialMediaPost", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaSourceSocialMediaPost", new[] { "SocialMediaSource_SourceId" });
            DropIndex("dbo.SocialMediaPostSocialMediaProfile", new[] { "SocialMediaProfile_Id" });
            DropIndex("dbo.SocialMediaPostSocialMediaProfile", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaPostPerson", new[] { "Person_ItemId" });
            DropIndex("dbo.SocialMediaPostPerson", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.SocialMediaPostHashtag", new[] { "Hashtag_Id" });
            DropIndex("dbo.SocialMediaPostHashtag", new[] { "SocialMediaPost_PostId" });
            DropIndex("dbo.ItemAlert", new[] { "Alert_AlertId" });
            DropIndex("dbo.ItemAlert", new[] { "Item_ItemId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.ChartItemData", new[] { "Item_ItemId" });
            DropIndex("dbo.Chart", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Chart", new[] { "Zone_Id" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Platform_Id" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Notification", new[] { "Alert_AlertId" });
            DropIndex("dbo.Keyword", new[] { "Theme_ItemId" });
            DropIndex("dbo.SocialMediaPost", new[] { "SocialMediaSource_SourceId" });
            DropIndex("dbo.SocialMediaPost", new[] { "SocialMediaProfile_Id" });
            DropIndex("dbo.SocialMediaPost", new[] { "PostSentiment_Id" });
            DropIndex("dbo.SocialMediaProfile", new[] { "Item_ItemId" });
            DropIndex("dbo.Item", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Item", new[] { "Organization_ItemId" });
            DropIndex("dbo.Alert", new[] { "User_Id" });
            DropIndex("dbo.Alert", new[] { "Item_ItemId" });
            DropIndex("dbo.Alert", new[] { "CompareItem_ItemId" });
            DropTable("dbo.PlatformApplicationUser");
            DropTable("dbo.PlatformSocialMediaSource");
            DropTable("dbo.ChartChartItemData");
            DropTable("dbo.ChartItemDataData");
            DropTable("dbo.ChartItem");
            DropTable("dbo.AlertNotification");
            DropTable("dbo.OrganizationSocialMediaProfile");
            DropTable("dbo.OrganizationPerson");
            DropTable("dbo.PersonSocialMediaProfile");
            DropTable("dbo.SocialMediaProfileSocialMediaPost");
            DropTable("dbo.SocialMediaPostWord");
            DropTable("dbo.SocialMediaPostUrl");
            DropTable("dbo.SocialMediaPostTheme");
            DropTable("dbo.SocialMediaSourceSocialMediaPost");
            DropTable("dbo.SocialMediaPostSocialMediaProfile");
            DropTable("dbo.SocialMediaPostPerson");
            DropTable("dbo.SocialMediaPostHashtag");
            DropTable("dbo.ItemAlert");
            DropTable("dbo.Roles");
            DropTable("dbo.Platform");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Zone");
            DropTable("dbo.Data");
            DropTable("dbo.ChartItemData");
            DropTable("dbo.Chart");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Notification");
            DropTable("dbo.Word");
            DropTable("dbo.Url");
            DropTable("dbo.Keyword");
            DropTable("dbo.SocialMediaSource");
            DropTable("dbo.Sentiment");
            DropTable("dbo.Hashtag");
            DropTable("dbo.SocialMediaPost");
            DropTable("dbo.SocialMediaProfile");
            DropTable("dbo.Item");
            DropTable("dbo.Alert");
        }
    }
}
