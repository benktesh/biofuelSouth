using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class CleanupGlossary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Glossaries", "Id", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Glossaries", "Id");
        }
    }
}
