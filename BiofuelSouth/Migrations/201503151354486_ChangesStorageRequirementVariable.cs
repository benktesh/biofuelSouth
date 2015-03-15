namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesStorageRequirementVariable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean(nullable: false));
        }
    }
}
