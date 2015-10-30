namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AnotherChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean());
        }
    }
}
