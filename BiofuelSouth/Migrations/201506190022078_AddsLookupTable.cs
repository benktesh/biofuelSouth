using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class AddsLookupTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LookUpEntities",
                c => new
                    {
                        Id = c.Guid(false),
                        LookUpId = c.Int(false),
                        Name = c.String(),
                        Description = c.String(),
                        Source = c.String(),
                        Label = c.String(),
                        Value = c.String(),
                        LookUpGroup = c.String(),
                        SortOrder = c.Int(false),
                        System = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LookUpEntities");
        }
    }
}
