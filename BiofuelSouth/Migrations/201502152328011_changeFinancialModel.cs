namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFinancialModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "LandCost", c => c.Double(nullable: false));
            DropColumn("dbo.Inputs", "Finance_LandCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "Finance_LandCost", c => c.Double(nullable: false));
            DropColumn("dbo.Inputs", "LandCost");
        }
    }
}
