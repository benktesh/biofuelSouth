using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class makesLatLonNullable : DbMigration
    {
        public override void Up()
        {
            /*
            AlterColumn("dbo.Counties", "Lat", c => c.Double());
            AlterColumn("dbo.Counties", "Lon", c => c.Double());
             * */
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counties", "Lon", c => c.Double(false));
            AlterColumn("dbo.Counties", "Lat", c => c.Double(false));
        }
    }
}
