namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCropTypeinProductivities : DbMigration
    {
        public override void Up()
        {

           


            Sql("Update Productivities Set CropType = 0 where CropType = 'Switchgrass';");
            Sql("Update Productivities Set CropType = 1 where CropType = 'Miscanthus';");
            Sql("Update Productivities Set CropType = 2 where CropType = 'Poplar';");
            Sql("Update Productivities Set CropType = 3 where CropType = 'Willow';");
            Sql("Update Productivities Set CropType = 4 where CropType = 'Pine';");


            AlterColumn("dbo.Productivities", "CropType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            Sql("Update Productivities Set CropType = 'Switchgrass' where CropType = 0;");
            Sql("Update Productivities Set CropType = 'Miscanthus' where CropType = 1;");
            Sql("Update Productivities Set CropType = 'Poplar' where CropType = 2;");
            Sql("Update Productivities Set CropType = 'Willow' where CropType = 3;");
            Sql("Update Productivities Set CropType = 'Pine' where CropType = 4;");

            AlterColumn("dbo.Productivities", "CropType", c => c.String());
        }
    }
}
