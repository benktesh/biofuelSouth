namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddsLookupTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LookUpEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LookUpId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Source = c.String(),
                        Label = c.String(),
                        Value = c.String(),
                        LookUpGroup = c.String(),
                        SortOrder = c.Int(nullable: false),
                        System = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LookUpEntities");
        }
    }
}
