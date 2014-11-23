namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makesLatLonNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counties", "Lat", c => c.Double());
            AlterColumn("dbo.Counties", "Lon", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counties", "Lon", c => c.Double(nullable: false));
            AlterColumn("dbo.Counties", "Lat", c => c.Double(nullable: false));
        }
    }
}
