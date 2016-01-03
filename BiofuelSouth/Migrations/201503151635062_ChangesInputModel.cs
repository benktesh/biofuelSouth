using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class ChangesInputModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inputs", "State");
            DropColumn("dbo.Inputs", "County");
            DropColumn("dbo.Inputs", "Category");
            DropColumn("dbo.Inputs", "ProjectSize");
            DropColumn("dbo.Inputs", "ProjectLife");
            DropColumn("dbo.Inputs", "BiomassPriceAtFarmGate");
            DropColumn("dbo.Inputs", "LandCost");
            DropColumn("dbo.Inputs", "ModelStorage");
            DropColumn("dbo.Inputs", "ModelFinancial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "ModelFinancial", c => c.Boolean());
            AddColumn("dbo.Inputs", "ModelStorage", c => c.Boolean());
            AddColumn("dbo.Inputs", "LandCost", c => c.Double(false));
            AddColumn("dbo.Inputs", "BiomassPriceAtFarmGate", c => c.Double(false));
            AddColumn("dbo.Inputs", "ProjectLife", c => c.Int(false));
            AddColumn("dbo.Inputs", "ProjectSize", c => c.Double(false));
            AddColumn("dbo.Inputs", "Category", c => c.String(false));
            AddColumn("dbo.Inputs", "County", c => c.String(false));
            AddColumn("dbo.Inputs", "State", c => c.String(false));
        }
    }
}
