namespace Geocache.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chained_Treasures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Treasure_1_ID = c.Int(),
                        Treasure_2_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Treasures", t => t.Treasure_1_ID)
                .ForeignKey("dbo.Treasures", t => t.Treasure_2_ID)
                .Index(t => t.Treasure_1_ID)
                .Index(t => t.Treasure_2_ID);
            
            CreateTable(
                "dbo.Treasures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TreasureType = c.Int(nullable: false),
                        TreasureSize = c.Int(nullable: false),
                        Description = c.String(),
                        Difficulty = c.Double(nullable: false),
                        Rating = c.Double(nullable: false),
                        Key = c.String(),
                        isChained = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        Chained_Treasures_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Chained_Treasures", t => t.Chained_Treasures_Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Chained_Treasures_Id);
            
            CreateTable(
                "dbo.Found_Treasures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        TreasureID = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Found_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Treasures", t => t.TreasureID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.TreasureID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                        isBanned = c.Boolean(nullable: false),
                        Points = c.Double(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Adress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Treasures_Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TreasureID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Content = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Rated = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Treasures", t => t.TreasureID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.TreasureID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.MarkerInfoes",
                c => new
                    {
                        TreasureId = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longtitude = c.Double(nullable: false),
                        City = c.String(),
                        Country = c.String(),
                        Adress = c.String(),
                    })
                .PrimaryKey(t => t.TreasureId)
                .ForeignKey("dbo.Treasures", t => t.TreasureId)
                .Index(t => t.TreasureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chained_Treasures", "Treasure_2_ID", "dbo.Treasures");
            DropForeignKey("dbo.Chained_Treasures", "Treasure_1_ID", "dbo.Treasures");
            DropForeignKey("dbo.Treasures", "UserId", "dbo.Users");
            DropForeignKey("dbo.MarkerInfoes", "TreasureId", "dbo.Treasures");
            DropForeignKey("dbo.Found_Treasures", "UserID", "dbo.Users");
            DropForeignKey("dbo.Treasures_Comments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Treasures_Comments", "TreasureID", "dbo.Treasures");
            DropForeignKey("dbo.Found_Treasures", "TreasureID", "dbo.Treasures");
            DropForeignKey("dbo.Treasures", "Chained_Treasures_Id", "dbo.Chained_Treasures");
            DropIndex("dbo.MarkerInfoes", new[] { "TreasureId" });
            DropIndex("dbo.Treasures_Comments", new[] { "UserID" });
            DropIndex("dbo.Treasures_Comments", new[] { "TreasureID" });
            DropIndex("dbo.Found_Treasures", new[] { "TreasureID" });
            DropIndex("dbo.Found_Treasures", new[] { "UserID" });
            DropIndex("dbo.Treasures", new[] { "Chained_Treasures_Id" });
            DropIndex("dbo.Treasures", new[] { "UserId" });
            DropIndex("dbo.Chained_Treasures", new[] { "Treasure_2_ID" });
            DropIndex("dbo.Chained_Treasures", new[] { "Treasure_1_ID" });
            DropTable("dbo.MarkerInfoes");
            DropTable("dbo.Treasures_Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Found_Treasures");
            DropTable("dbo.Treasures");
            DropTable("dbo.Chained_Treasures");
        }
    }
}
