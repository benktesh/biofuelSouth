namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Inputs", "County", c => c.String(nullable: false));
            AlterColumn("dbo.Inputs", "State", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "State", c => c.String());
            AlterColumn("dbo.Inputs", "County", c => c.String());
            AlterColumn("dbo.Inputs", "Category", c => c.String());
        }
    }
}
