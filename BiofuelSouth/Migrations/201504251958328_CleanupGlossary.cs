namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
