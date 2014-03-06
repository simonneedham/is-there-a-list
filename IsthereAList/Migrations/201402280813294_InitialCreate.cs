namespace IsThereAList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListItem",
                c => new
                    {
                        ListItemId = c.Int(nullable: false, identity: true),
                        ListId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Url = c.String(),
                        PictureUrl = c.String(),
                        ApplicationUserIdPurchased = c.String(maxLength: 128),
                        Deleted = c.Boolean(nullable: false),
                        ApplicationUserIdUpdated = c.String(nullable: false, maxLength: 128),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ListItemId)
                .ForeignKey("dbo.List", t => t.ListId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserIdPurchased)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserIdUpdated, cascadeDelete: true)
                .Index(t => t.ListId)
                .Index(t => t.ApplicationUserIdPurchased)
                .Index(t => t.ApplicationUserIdUpdated);
            
            CreateTable(
                "dbo.List",
                c => new
                    {
                        ListId = c.Int(nullable: false, identity: true),
                        ListTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                        EffectiveDate = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ListId)
                .ForeignKey("dbo.ListType", t => t.ListTypeId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: false)
                .Index(t => t.ListTypeId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.ListType",
                c => new
                    {
                        ListTypeId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ListTypeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        SendEmails = c.Boolean(),
                        DobDay = c.Int(),
                        DobMonth = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListItem", "ApplicationUserIdUpdated", "dbo.AspNetUsers");
            DropForeignKey("dbo.ListItem", "ApplicationUserIdPurchased", "dbo.AspNetUsers");
            DropForeignKey("dbo.List", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.List", "ListTypeId", "dbo.ListType");
            DropForeignKey("dbo.ListItem", "ListId", "dbo.List");
            DropIndex("dbo.ListItem", new[] { "ApplicationUserIdUpdated" });
            DropIndex("dbo.ListItem", new[] { "ApplicationUserIdPurchased" });
            DropIndex("dbo.List", new[] { "OwnerId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.List", new[] { "ListTypeId" });
            DropIndex("dbo.ListItem", new[] { "ListId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ListType");
            DropTable("dbo.List");
            DropTable("dbo.ListItem");
        }
    }
}
