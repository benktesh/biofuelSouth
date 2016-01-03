using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class anotherModelChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean(false));
        }
    }
}
