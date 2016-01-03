using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class glossaryModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Glossaries", "source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Glossaries", "source");
        }
    }
}
