namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ReviseGlossary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Glossaries", "ModifiedBy", c => c.String());
            AddColumn("dbo.Glossaries", "CreatedBy", c => c.String());
            AddColumn("dbo.Glossaries", "Created", c => c.DateTime());
            AddColumn("dbo.Glossaries", "Modified", c => c.DateTime());
            AddColumn("dbo.Glossaries", "IsDirty", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Glossaries", "IsDirty");
            DropColumn("dbo.Glossaries", "Modified");
            DropColumn("dbo.Glossaries", "Created");
            DropColumn("dbo.Glossaries", "CreatedBy");
            DropColumn("dbo.Glossaries", "ModifiedBy");
        }
    }
}
