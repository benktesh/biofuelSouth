using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class updateGeoID : DbMigration
    {
        public override void Up()
        {
            /*
            AddColumn("dbo.Counties", "GeoID", c => c.String());
             * */
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counties", "GeoID");
        }
    }
}
