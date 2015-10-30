namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AnotherChange1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Storage_StorageMethod", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "Storage_StorageMethod", c => c.String());
        }
    }
}
