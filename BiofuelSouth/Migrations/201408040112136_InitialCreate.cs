namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            /*
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountyCode = c.Int(nullable: false),
                        State = c.String(),
                        Lat = c.Double(nullable: false),
                        Lon = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Productivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountyId = c.Int(nullable: false),
                        CropType = c.String(),
                        Yield = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
             * */
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Productivities");
            DropTable("dbo.Counties");
        }
    }
}
