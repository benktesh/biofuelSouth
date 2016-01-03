using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class changeInputModel : DbMigration
    {
        public override void Up()
        {
            /*
            AddColumn("dbo.Inputs", "County", c => c.String());
            DropColumn("dbo.Inputs", "CountyCode");
             * */
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "CountyCode", c => c.Int(false));
            DropColumn("dbo.Inputs", "County");
        }
    }
}
