namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddsStorageCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "Storage_CostOption", c => c.Int(nullable: false));
            AddColumn("dbo.Inputs", "Storage_Description", c => c.String());
            AddColumn("dbo.Inputs", "Storage_PalletCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inputs", "Storage_TarpCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inputs", "Storage_GravelCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inputs", "Storage_LaborCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inputs", "Storage_LandCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inputs", "Storage_UserEstimatedCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
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
