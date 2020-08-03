namespace Geocache.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Points", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "createdAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Points", c => c.Double(nullable: false));
        }
    }
}
