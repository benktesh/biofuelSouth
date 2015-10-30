namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
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
            AddColumn("dbo.Inputs", "LandCost", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "BiomassPriceAtFarmGate", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "ProjectLife", c => c.Int(nullable: false));
            AddColumn("dbo.Inputs", "ProjectSize", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Category", c => c.String(nullable: false));
            AddColumn("dbo.Inputs", "County", c => c.String(nullable: false));
            AddColumn("dbo.Inputs", "State", c => c.String(nullable: false));
        }
    }
}
