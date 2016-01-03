using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class AnotherChange2 : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.Glossaries");
            //AlterColumn("dbo.Glossaries", "Term", c => c.String());
            //AlterColumn("dbo.Glossaries", "Id", c => c.Guid(nullable: false));
            //AddPrimaryKey("dbo.Glossaries", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Glossaries");
            AlterColumn("dbo.Glossaries", "Id", c => c.Guid());
            AlterColumn("dbo.Glossaries", "Term", c => c.String(false, 128));
            AddPrimaryKey("dbo.Glossaries", "Term");
        }
    }
}
