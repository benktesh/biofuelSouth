namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesStorageAndGeneralModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Generals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        County = c.String(nullable: false),
                        Category = c.String(nullable: false),
                        ProjectSize = c.Double(nullable: false),
                        ProjectLife = c.Int(nullable: false),
                        BiomassPriceAtFarmGate = c.Double(nullable: false),
                        LandCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Inputs", "ModelStorage", c => c.Boolean());
            AddColumn("dbo.Inputs", "ModelFinancial", c => c.Boolean());
            AddColumn("dbo.Inputs", "Storage_RequireStorage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inputs", "Storage_StorageTime", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Storage_PercentDirectlyToPlantGate", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Storage_PercentStored", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Storage_StorageMethod", c => c.String());
            AddColumn("dbo.Inputs", "General_Id", c => c.Int());
            CreateIndex("dbo.Inputs", "General_Id");
            AddForeignKey("dbo.Inputs", "General_Id", "dbo.Generals", "Id");
            DropColumn("dbo.Inputs", "StorageRequirement_RequireStorage");
            DropColumn("dbo.Inputs", "StorageRequirement_StorageTime");
            DropColumn("dbo.Inputs", "StorageRequirement_PercentDirectlyToPlantGate");
            DropColumn("dbo.Inputs", "StorageRequirement_PercentStored");
            DropColumn("dbo.Inputs", "StorageRequirement_StorageMethod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "StorageRequirement_StorageMethod", c => c.String());
            AddColumn("dbo.Inputs", "StorageRequirement_PercentStored", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "StorageRequirement_PercentDirectlyToPlantGate", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "StorageRequirement_StorageTime", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "StorageRequirement_RequireStorage", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Inputs", "General_Id", "dbo.Generals");
            DropIndex("dbo.Inputs", new[] { "General_Id" });
            DropColumn("dbo.Inputs", "General_Id");
            DropColumn("dbo.Inputs", "Storage_StorageMethod");
            DropColumn("dbo.Inputs", "Storage_PercentStored");
            DropColumn("dbo.Inputs", "Storage_PercentDirectlyToPlantGate");
            DropColumn("dbo.Inputs", "Storage_StorageTime");
            DropColumn("dbo.Inputs", "Storage_RequireStorage");
            DropColumn("dbo.Inputs", "ModelFinancial");
            DropColumn("dbo.Inputs", "ModelStorage");
            DropTable("dbo.Generals");
        }
    }
}
