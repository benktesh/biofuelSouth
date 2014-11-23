namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInputModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "County", c => c.String());
            DropColumn("dbo.Inputs", "CountyCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "CountyCode", c => c.Int(nullable: false));
            DropColumn("dbo.Inputs", "County");
        }
    }
}
