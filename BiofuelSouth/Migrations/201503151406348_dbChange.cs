using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class dbChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean(false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean());
        }
    }
}
