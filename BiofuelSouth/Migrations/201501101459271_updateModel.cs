using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class updateModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inputs", "Category", c => c.String(false));
            AlterColumn("dbo.Inputs", "County", c => c.String(false));
            AlterColumn("dbo.Inputs", "State", c => c.String(false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inputs", "State", c => c.String());
            AlterColumn("dbo.Inputs", "County", c => c.String());
            AlterColumn("dbo.Inputs", "Category", c => c.String());
        }
    }
}
