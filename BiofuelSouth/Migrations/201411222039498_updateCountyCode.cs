namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCountyCode : DbMigration
    {
        public override void Up()
        {
            /*
            AlterColumn("dbo.Counties", "CountyCode", c => c.String());
             * */
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counties", "CountyCode", c => c.Int(nullable: false));
        }
    }
}
