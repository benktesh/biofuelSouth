using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class ChangesStorageRequirementVariable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean(false));
        }
    }
}
