using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class AddsStorageCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "Storage_CostOption", c => c.Int(false));
            AddColumn("dbo.Inputs", "Storage_Description", c => c.String());
            AddColumn("dbo.Inputs", "Storage_PalletCost", c => c.Decimal(false, 18, 2));
            AddColumn("dbo.Inputs", "Storage_TarpCost", c => c.Decimal(false, 18, 2));
            AddColumn("dbo.Inputs", "Storage_GravelCost", c => c.Decimal(false, 18, 2));
            AddColumn("dbo.Inputs", "Storage_LaborCost", c => c.Decimal(false, 18, 2));
            AddColumn("dbo.Inputs", "Storage_LandCost", c => c.Decimal(false, 18, 2));
            AddColumn("dbo.Inputs", "Storage_UserEstimatedCost", c => c.Decimal(false, 18, 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inputs", "Storage_UserEstimatedCost");
            DropColumn("dbo.Inputs", "Storage_LandCost");
            DropColumn("dbo.Inputs", "Storage_LaborCost");
            DropColumn("dbo.Inputs", "Storage_GravelCost");
            DropColumn("dbo.Inputs", "Storage_TarpCost");
            DropColumn("dbo.Inputs", "Storage_PalletCost");
            DropColumn("dbo.Inputs", "Storage_Description");
            DropColumn("dbo.Inputs", "Storage_CostOption");
        }
    }
}
