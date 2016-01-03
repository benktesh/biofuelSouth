using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class changeFinancialModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "LandCost", c => c.Double(false));
            DropColumn("dbo.Inputs", "Finance_LandCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "Finance_LandCost", c => c.Double(false));
            DropColumn("dbo.Inputs", "LandCost");
        }
    }
}
