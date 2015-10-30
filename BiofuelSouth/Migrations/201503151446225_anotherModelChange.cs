namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class anotherModelChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean(nullable: false));
        }
    }
}
