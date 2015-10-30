namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class dbChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean());
        }
    }
}
