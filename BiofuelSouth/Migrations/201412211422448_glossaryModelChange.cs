namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
